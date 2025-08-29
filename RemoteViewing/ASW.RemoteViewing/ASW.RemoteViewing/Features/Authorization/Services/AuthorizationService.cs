using ASW.RemoteViewing.Features.Authorization.Claims;
using ASW.RemoteViewing.Features.User.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Infrastructure.Security.Key;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests;
using ASW.RemoteViewing.Shared.Responses.Authentication;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ASW.RemoteViewing.Features.Authorization.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly KeyService _keyService; 
        PgDbContext _context;
        public AuthorizationService(PgDbContext context, KeyService keyService) 
        {
            _context = context;
            _keyService = keyService; 
        }
       
        public async Task<UserInfoDto?> ValidateTokenState(string authorizeHeader)
        {
            var handler = new JsonWebTokenHandler();
            var token = handler.ReadJsonWebToken(authorizeHeader.TrimStart('"').TrimEnd('"').Replace("Bearer ", ""));
            var idUserClaim = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var modifiedIdClaim = token.Claims.FirstOrDefault(x => x.Type == "modified_id");

            if (idUserClaim?.Value is null || modifiedIdClaim?.Value is null)
                throw new ValidationException("Повторите вход");

            if (!Guid.TryParse(idUserClaim.Value, out var id) ||
                !Guid.TryParse(modifiedIdClaim.Value, out var modifiedId))
                throw new ValidationException("Недопустимый токен. Повторите вход.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new ValidationException("Пользователя не существует");
            if (user.ModifiedId != modifiedId)
                throw new ValidationException("Повторите вход, данные были изменены");

            if (user.IsRemoved == true)
                throw new ValidationException("Нет доступа");

            return new UserInfoDto
            {
                Id = user.Id,
                FullFIO = user.FullFIO,
                ReductionFIO = user.ReductionFIO,
                Login = user.Login,
                Role = user.Role
            };
        }

        public async Task<LoginResponse> LogIn(LoginRequest userLogin)
        { 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin.Login)
                ?? throw new ValidationException("Не верный логин");
            if (!PassHelper<UserEF>.Verify(user, user.Password, userLogin.Password))
                throw new ValidationException("Не верный пароль");

            if (user.IsRemoved == true)
                throw new ValidationException("Нет доступа");

            var handler = new JsonWebTokenHandler();
            var key = new RsaSecurityKey(_keyService.RsaKey1); 
            var claims = ClaimsPrincipalFactory.CreateClaims(new AuthClaimsInfo
            {
                Id = user.Id,
                Name = user.ReductionFIO,
                Role = user.Role,
                ModifiedId = user.ModifiedId,
                AuthScheme = AuthSchemes.UserJwt
            });
            var identity = new ClaimsIdentity(claims);
            var token = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = "ASW",
                Audience = "UserAccess",
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(90),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
            });
           
            return new LoginResponse { Token = token };
        }
        
    }
}
