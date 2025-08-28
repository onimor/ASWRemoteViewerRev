using ASW.RemoteViewing.Features.Authorization.Services;
using ASW.RemoteViewing.Features.User.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Infrastructure.Security.Key;
using ASW.RemoteViewing.Shared.Requests;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Extentions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ASW.RemoteViewing.Tests.Unit.Authorization
{
    public class AuthorizationServiceTests
    {
        private PgDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PgDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new PgDbContext(options);
        }
        private static string GenerateJwt(Guid id, Guid modifiedId)
        {
            var handler = new JsonWebTokenHandler();
            var rsa = RSA.Create(); // временный ключ
            var key = new RsaSecurityKey(rsa);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
        new Claim("modified_id", modifiedId.ToString())
    };

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
            });

            return token;
        }

        [Fact(DisplayName = "LogIn: возвращает JWT, если логин и пароль валидны")]
        public async Task LogIn_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange:
            // Создаём пользователя с валидным логином и паролем
            // и подменяем валидацию пароля, чтобы всегда возвращала true
            var context = CreateInMemoryContext("ValidLoginTest");

            var user = new UserEF
            {
                Id = Guid.NewGuid(),
                Login = "test",
                Password = "hash", // захардкожено, но не важно — валидация подменяется
                Role = "Admin",
                FullFIO = "Тестов Тест Тестович",
                ReductionFIO = "Т.Т.Т.",
                IsRemoved = false,
                ModifiedId = Guid.NewGuid()
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var keyService = new TestKeyService();
            PassHelper<UserEF>.SetTestVerifier((u, h, p) => true);

            var service = new AuthorizationService(context, keyService);
            var request = new LoginRequest { Login = "test", Password = "any" };

            // Act:
            var response = await service.LogIn(request);

            // Assert:
            // Убедимся, что возвращён не пустой токен
            response.Token.Should().NotBeNullOrEmpty();

            PassHelper<UserEF>.ResetTestVerifier();
        }

        [Fact(DisplayName = "LogIn: исключение, если пароль неверный")]
        public async Task LogIn_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange:
            // Создаём пользователя и подменяем проверку пароля на "всегда false"
            var context = CreateInMemoryContext("InvalidPasswordTest");

            var user = new UserEF
            {
                Id = Guid.NewGuid(),
                Login = "test",
                Password = "hash",
                Role = "Admin",
                IsRemoved = false,
                ReductionFIO = "Тест",
                ModifiedId = Guid.NewGuid()
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var keyService = new TestKeyService();
            PassHelper<UserEF>.SetTestVerifier((u, h, p) => false); // неверный пароль

            var service = new AuthorizationService(context, keyService);
            var request = new LoginRequest { Login = "test", Password = "wrong" };

            // Act:
            var act = async () => await service.LogIn(request);

            // Assert:
            // Ожидаем ValidationException с текстом "Не верный пароль"
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Не верный пароль");

            PassHelper<UserEF>.ResetTestVerifier();
        }

        [Fact(DisplayName = "LogIn: исключение, если пользователь не существует")]
        public async Task LogIn_ShouldThrowException_WhenLoginNotFound()
        {
            // Arrange:
            // База пустая, логин "missing_user" не существует
            var context = CreateInMemoryContext("LoginNotFoundTest");

            var keyService = new TestKeyService();
            var service = new AuthorizationService(context, keyService);
            var request = new LoginRequest { Login = "missing_user", Password = "any" };

            // Act:
            var act = async () => await service.LogIn(request);

            // Assert:
            // Ожидаем ValidationException с текстом "Не верный логин"
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Не верный логин");
        }

        [Fact(DisplayName = "LogIn: исключение, если пользователь удалён")]
        public async Task LogIn_ShouldThrowException_WhenUserIsRemoved()
        {
            // Arrange:
            // Пользователь помечен как удалённый (IsRemoved = true)
            var context = CreateInMemoryContext("RemovedUserTest");

            var user = new UserEF
            {
                Id = Guid.NewGuid(),
                Login = "test",
                Password = "hash",
                Role = "Admin",
                IsRemoved = true, // ❗️ удалён
                ReductionFIO = "X",
                ModifiedId = Guid.NewGuid()
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var keyService = new TestKeyService();
            PassHelper<UserEF>.SetTestVerifier((u, h, p) => true); // даже если пароль верный

            var service = new AuthorizationService(context, keyService);
            var request = new LoginRequest { Login = "test", Password = "any" };

            // Act:
            var act = async () => await service.LogIn(request);

            // Assert:
            // Ожидаем ValidationException с текстом "Нет доступа"
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Нет доступа");

            PassHelper<UserEF>.ResetTestVerifier();
        }

        [Fact(DisplayName = "ValidateTokenState: возвращает UserInfoDto, если токен валиден и пользователь существует")]
        public async Task ValidateTokenState_ShouldReturnUser_WhenTokenIsValid()
        {
            // Arrange
            var context = CreateInMemoryContext("ValidUserInfoTest");

            var userId = Guid.NewGuid();
            var modifiedId = Guid.NewGuid();

            var user = new UserEF
            {
                Id = userId,
                ModifiedId = modifiedId,
                Login = "test",
                Role = "Admin",
                FullFIO = "Тестов Тест",
                ReductionFIO = "Т.Т.",
                IsRemoved = false,
                Password = "doesnt_matter"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var token = GenerateJwt(userId, modifiedId); // с нужными клеймами
            var authHeader = $"Bearer {token}";

            var keyService = new TestKeyService();
            var service = new AuthorizationService(context, keyService);

            // Act
            var result = await service.ValidateTokenState(authHeader);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            result.Login.Should().Be("test");
            result.Role.Should().Be("Admin");
        }

        [Fact(DisplayName = "ValidateTokenState: исключение, если в токене отсутствуют обязательные клеймы")]
        public async Task ValidateTokenState_ShouldThrowException_WhenClaimsAreMissing()
        {
            // Arrange
            var handler = new JsonWebTokenHandler();
            var rsa = RSA.Create();
            var key = new RsaSecurityKey(rsa);

            // Токен без NameIdentifier и modified_id
            var claims = new[]
            {
                new Claim("some", "value") // невалидные клеймы
    };

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
            });

            var context = CreateInMemoryContext("MissingClaimsTest");
            var keyService = new TestKeyService();
            var service = new AuthorizationService(context, keyService);
            var authHeader = $"Bearer {token}";

            // Act
            var act = async () => await service.ValidateTokenState(authHeader);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Повторите вход");
        }

        [Fact(DisplayName = "ValidateTokenState: исключение, если пользователь не найден по Id")]
        public async Task ValidateTokenState_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext("UserNotFoundTest");

            var userId = Guid.NewGuid();
            var modifiedId = Guid.NewGuid();

            var token = GenerateJwt(userId, modifiedId);
            var authHeader = $"Bearer {token}";

            var keyService = new TestKeyService();
            var service = new AuthorizationService(context, keyService);

            // Act
            var act = async () => await service.ValidateTokenState(authHeader);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Пользователя не существует");
        }

        [Fact(DisplayName = "ValidateTokenState: исключение, если ModifiedId не совпадает")]
        public async Task ValidateTokenState_ShouldThrowException_WhenModifiedIdMismatch()
        {
            // Arrange
            var context = CreateInMemoryContext("ModifiedIdMismatchTest");

            var userId = Guid.NewGuid();
            var realModifiedId = Guid.NewGuid();
            var fakeModifiedId = Guid.NewGuid(); // ❗ подделка

            var user = new UserEF
            {
                Id = userId,
                ModifiedId = realModifiedId,
                Login = "test",
                Role = "User",
                FullFIO = "X",
                ReductionFIO = "X",
                IsRemoved = false
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var token = GenerateJwt(userId, fakeModifiedId); // токен с неверным ModifiedId
            var authHeader = $"Bearer {token}";

            var keyService = new TestKeyService();
            var service = new AuthorizationService(context, keyService);

            // Act
            var act = async () => await service.ValidateTokenState(authHeader);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Повторите вход, данные были изменены");
        }

        [Fact(DisplayName = "ValidateTokenState: исключение, если пользователь помечен как удалённый")]
        public async Task ValidateTokenState_ShouldThrowException_WhenUserIsRemoved()
        {
            // Arrange
            var context = CreateInMemoryContext("UserIsRemovedTest");

            var userId = Guid.NewGuid();
            var modifiedId = Guid.NewGuid();

            var user = new UserEF
            {
                Id = userId,
                ModifiedId = modifiedId,
                Login = "test",
                Role = "User",
                FullFIO = "Удалённый",
                ReductionFIO = "У.Д.",
                IsRemoved = true // ❗️
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var token = GenerateJwt(userId, modifiedId);
            var authHeader = $"Bearer {token}";

            var keyService = new TestKeyService();
            var service = new AuthorizationService(context, keyService);

            // Act
            var act = async () => await service.ValidateTokenState(authHeader);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Нет доступа");
        }

    }
}
