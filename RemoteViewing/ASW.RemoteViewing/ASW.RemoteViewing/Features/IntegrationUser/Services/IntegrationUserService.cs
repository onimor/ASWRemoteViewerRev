using ASW.RemoteViewing.Features.Authorization.UserContext;
using ASW.RemoteViewing.Features.IntegrationUser.Entities;
using ASW.RemoteViewing.Features.IntegrationUser.Mappings;
using ASW.RemoteViewing.Features.IntegrationUser.Services;
using ASW.RemoteViewing.Features.PlaceUser.Mapping;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Dto.IntegrationUser;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;
using ASW.RemoteViewing.Shared.Security;
using ASW.RemoteViewing.Shared.Utilities;
using ASW.Shared.Extentions;
using Microsoft.EntityFrameworkCore;

public class IntegrationUserService : IIntegrationUserService
{
    private readonly IUserContext _userContext;
    private readonly PgDbContext _pgDbContext;

    public IntegrationUserService(IUserContext userContext, PgDbContext pgDbContext)
    {
        _pgDbContext = pgDbContext;
        _userContext = userContext;
    }
 
    public async Task<LoginResponse> CreateAsync(CreateIntegrationUserRequest addApiKeyClientRequest)
    {
        var key = KeyGenerator.GenerateKey();
        var keyPrefix = key[^12..];
        if (await _pgDbContext.IntegrationUsers.AnyAsync(x => x.KeyPrefix == keyPrefix))
            throw new ValidationException("Конфликт ключа. Повторите попытку.");

        var newIntegrationUser = new IntegrationUserEF
        {
            Name = addApiKeyClientRequest.Name,
            KeyPrefix = keyPrefix,
            GeneratedByUserId = _userContext.UserId,
            GeneratedByUserName = _userContext.UserName,
            KeyGeneratedAt = DateTime.UtcNow,
            Role = Roles.Integration,
        }; 

        newIntegrationUser.KeyHash = PassHelper<IntegrationUserEF>.GetHash(newIntegrationUser, key);
        _pgDbContext.IntegrationUsers.Add(newIntegrationUser);
        await _pgDbContext.SaveChangesAsync();
        return new LoginResponse { Token = key };
    }

    public async Task DeleteAsync(Guid integrationUserId)
    {
        var integrationUser = await _pgDbContext.IntegrationUsers
            .FirstOrDefaultAsync(x => x.Id == integrationUserId);

        if (integrationUser is not null)
        {
            integrationUser.IsRemoved = true;
            await _pgDbContext.SaveChangesAsync();
        }
    }

    public async Task<List<IntegrationUserDto>> GetAllAsync()
    {
        var list = await _pgDbContext.IntegrationUsers
            .Where(x => x.IsRemoved == false)
            .ToListAsync();

        return list.Select(x=>x.ToDto()).ToList();
    }

    public async Task<IntegrationUserDto?> GetByIdAsync(Guid integrationUserId)
    {
        var integrationUser = await _pgDbContext.IntegrationUsers
            .FirstOrDefaultAsync(x => x.Id == integrationUserId);

        return integrationUser is null ? null : integrationUser.ToDto();
    }

    public async Task<IntegrationUserDto?> GetByKey(string key)
    {
        var keyPrefix = key[^12..];
        var integrationUser = await _pgDbContext.IntegrationUsers
            .FirstOrDefaultAsync(x => x.KeyPrefix == keyPrefix && x.IsRemoved == false);

        if (integrationUser is null)
            return null;

        var isVerify = PassHelper<IntegrationUserEF>.Verify(integrationUser, integrationUser.KeyHash, key);
        if (isVerify == false)
            throw new ValidationException("Нет доступа");

        return integrationUser.ToDto();
    }

    public async Task UpdateAsync(Guid integrationUserId, UpdateIntegrationUserRequest updateIntegrationUserRequest)
    {
        var integrationUser = await _pgDbContext.IntegrationUsers
            .FirstOrDefaultAsync(x => x.Id == integrationUserId);

        if (integrationUser is not null)
        {
            integrationUser.Name = updateIntegrationUserRequest.Name;
            await _pgDbContext.SaveChangesAsync();
        }
    }

    public async Task<LoginResponse> UpdateKeyAsync(Guid integrationUserId)
    {
        var key = KeyGenerator.GenerateKey();
        var keyPrefix = key[^12..];
        if (await _pgDbContext.IntegrationUsers.AnyAsync(x => x.KeyPrefix == keyPrefix))
            throw new ValidationException("Конфликт ключа. Повторите попытку.");

        var integrationUser = await _pgDbContext.IntegrationUsers
            .FirstOrDefaultAsync(x => x.Id == integrationUserId)
            ?? throw new ValidationException("Не найдено");

        integrationUser.KeyPrefix = keyPrefix;
        integrationUser.GeneratedByUserId = _userContext.UserId;
        integrationUser.GeneratedByUserName = _userContext.UserName;
        integrationUser.KeyGeneratedAt = DateTime.UtcNow;
        integrationUser.KeyHash = PassHelper<IntegrationUserEF>.GetHash(integrationUser, key);

        await _pgDbContext.SaveChangesAsync();
        return new LoginResponse { Token = key };
    }
}
