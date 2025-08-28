using ASW.RemoteViewing.Infrastructure.Data.Base; 
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Shared.BaseServices;

/// <summary>
/// Универсальный базовый сервис для работы с сущностями.
/// </summary>
public abstract class EntityServiceBase<TContext, TEntity, TDto, TId>
    where TContext : DbContext
    where TEntity : class, IEntity<TId>, ISoftRemovable
{
    protected readonly TContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly IMapper _mapper;

    protected EntityServiceBase(TContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все активные сущности.
    /// </summary>
    public virtual async Task<List<TDto>> GetAllAsync()
    {
        return await _dbSet
            .Where(x => !x.IsRemoved)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <summary>
    /// Получить одну активную сущность по Id.
    /// </summary>
    public virtual async Task<TDto?> GetByIdAsync(TId id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id) && !x.IsRemoved);
        return entity == null ? default : _mapper.Map<TDto>(entity);
    }

    /// <summary>
    /// Мягкое удаление по Id.
    /// </summary>
    public virtual async Task DeleteAsync(TId id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id));
        if (entity is not null)
        {
            entity.IsRemoved = true;
            await SaveAsync();
        }
    }

    /// <summary>
    /// Сохраняет изменения.
    /// </summary>
    protected virtual Task SaveAsync() => _context.SaveChangesAsync();
}