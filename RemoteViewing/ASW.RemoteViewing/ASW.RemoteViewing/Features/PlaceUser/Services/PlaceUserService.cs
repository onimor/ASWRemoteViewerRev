using ASW.RemoteViewing.Features.Authorization;
using ASW.RemoteViewing.Features.Authorization.Claims;
using ASW.RemoteViewing.Features.Authorization.CurrentUser.Default;
using ASW.RemoteViewing.Features.PlaceUser.Entities;
using ASW.RemoteViewing.Features.PlaceUser.Mapping;
using ASW.RemoteViewing.Features.User.Mapping;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Infrastructure.Security.Key;
using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Dto.PlaceUser;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ASW.RemoteViewing.Features.PlaceUser.Services;

public class PlaceUserService : IPlaceUserService
{
    private readonly ICurrentUserProvider _userProvider;
    private readonly PgDbContext _pgDbContext;
    private readonly KeyService _keyService;

    public PlaceUserService(ICurrentUserProvider userProvider, PgDbContext pgDbContext, KeyService keyService)
    {
        _pgDbContext = pgDbContext;
        _userProvider = userProvider;
        _keyService = keyService;
    }

    public async Task<LoginResponse> CreateAsync(CreatePlaceUserRequest addPlaceUserRequest)
    {
        var currentUser = await _userProvider.GetCurrentUserAsync();
        var newPlaceUser = new PlaceUserEF
        {
            Name = addPlaceUserRequest.Name,
            CreatedAt = DateTime.UtcNow,
            ModifiedId = Guid.NewGuid(),
            ModifiedAt = DateTime.UtcNow,
            ModifiedByUserId = currentUser.Id,
            ModifiedByUserName = currentUser.ReductionFIO,
            Role = Roles.PlaceClient
        };
        _pgDbContext.PlaceUsers.Add(newPlaceUser);
        await _pgDbContext.SaveChangesAsync();

        return new LoginResponse { Token = GenerateJWT(newPlaceUser) };
    }

    public async Task DeleteAsync(Guid placeUserId)
    {
        var placeUser = await _pgDbContext.PlaceUsers.FirstOrDefaultAsync(x => x.Id == placeUserId);
        if (placeUser is not null)
        {
            placeUser.IsRemoved = true;
            await _pgDbContext.SaveChangesAsync();
        }
    }

    public async Task<List<PlaceUserDto>> GetAllAsync()
    {
        var list = await _pgDbContext.PlaceUsers
            .Where(x => x.IsRemoved == false)
            .ToListAsync();

        return list.Select(x => x.ToDto()).ToList();
    }

    public async Task<PlaceUserDto?> GetByIdAsync(Guid placeUserId)
    {
        var placeUser = await _pgDbContext.PlaceUsers
            .FirstOrDefaultAsync(x => x.Id == placeUserId && x.IsRemoved == false);

        return placeUser?.ToDto();
    }

    public async Task UpdateAsync(Guid placeUserId, UpdatePlaceUserRequest updatePlaceUserRequest)
    {
        var placeUser = await _pgDbContext.PlaceUsers
            .FirstOrDefaultAsync(x => x.Id == placeUserId && x.IsRemoved == false);

        if (placeUser is not null)
        {
            placeUser.Name = updatePlaceUserRequest.Name;
            await _pgDbContext.SaveChangesAsync();
        }
    }

    public async Task<LoginResponse> UpdateJWT(Guid placeUserId)
    {
        var integrationUser = await _pgDbContext.PlaceUsers
            .FirstOrDefaultAsync(x => x.Id == placeUserId && x.IsRemoved == false)
            ?? throw new ValidationException("Не найдено");

        var currentUser = await _userProvider.GetCurrentUserAsync();
        integrationUser.ModifiedId = Guid.NewGuid();
        integrationUser.ModifiedByUserId = currentUser.Id;
        integrationUser.ModifiedByUserName = currentUser.ReductionFIO;
        integrationUser.ModifiedAt = DateTime.UtcNow;
        await _pgDbContext.SaveChangesAsync();
        return new LoginResponse { Token = GenerateJWT(integrationUser) };
    }

    private string GenerateJWT(PlaceUserEF placeUser)
    {
        var handler = new JsonWebTokenHandler();
        var key = new RsaSecurityKey(_keyService.RsaKey3);

        var claims = ClaimsPrincipalFactory.CreateClaims(new AuthClaimsInfo
        {
            Id = placeUser.Id,
            Name = placeUser.Name,
            Role = placeUser.Role,
            ModifiedId = placeUser.ModifiedId,
            AuthScheme = AuthSchemes.PlaceUserJwt,
            ExtraClaims = new()
            {
                ["type"] = "service",
            }
        });
        var identity = new ClaimsIdentity(claims);
        var token = handler.CreateToken(new SecurityTokenDescriptor()
        {
            Issuer = "ASW",
            Audience = "PlaceClientAccess",
            Subject = identity,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
        });
        return token;
    }
}
