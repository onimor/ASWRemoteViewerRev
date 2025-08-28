using ASW.RemoteViewing.Features.RemotePostUsers.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.RemotePostUsers;
using ASW.Shared.Requests.RemotePostUsers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemotePostUsers.Services;

public class RemotePostUsersService : IRemotePostUsersService
{
    private readonly PgDbContext _context;
    protected readonly IMapper _mapper;
    public RemotePostUsersService(PgDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task CreateAsync(AddRemotePostUsersRequest addPostUsersRequest)
    { 
        var postUser = new RemotePostUserEF
        {
            PostId = addPostUsersRequest.PostId,
            PostName = addPostUsersRequest.PostName,
            UserId = addPostUsersRequest.UserId,
            UserName = addPostUsersRequest.UserName,
        };
        await _context.RemotePostUsers.AddAsync(postUser); 
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByPostAsync(Guid postId)
    { 
        await _context.RemotePostUsers.Where(x => x.PostId == postId).ExecuteDeleteAsync();
    }

    public async Task<List<RemotePostUserDto>?> GetByPostAsync(Guid postId)
    { 
        return await _context.RemotePostUsers
            .Where(x => x.PostId == postId)
            .ProjectTo<RemotePostUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<RemotePostUserDto>?> GetByUserAsync(Guid userId)
    { 
        return await _context.RemotePostUsers
            .Where(x => x.UserId == userId)
            .ProjectTo<RemotePostUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
