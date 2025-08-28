using ASW.RemoteViewing.Features.Authorization.CurrentUser.Default;
using ASW.RemoteViewing.Infrastructure.Data.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Shared.BaseServices;

public abstract class EntityMainBaseService<TContext, TEntity, TDto, TCreateRequest, TUpdateRequest>
    : EntityServiceBase<TContext, TEntity, TDto, Guid>
    where TContext : DbContext
    where TEntity : class, IEntity<Guid>, ISoftRemovable, new()
{
    protected readonly ICurrentUserProvider _userProvider;

    protected EntityMainBaseService(
        TContext context,
        ICurrentUserProvider userProvider,
        IMapper mapper)
        : base(context, mapper)
    {
        _userProvider = userProvider;
    }

    /// <summary>
    /// Создание новой сущности.
    /// </summary>
    public virtual async Task<TCreateRequest> CreateAsync(TCreateRequest request)
    {
        var entity = await MapFromCreateRequestAsync(request);

        await _dbSet.AddAsync(entity);
        await SaveAsync();

        return request;
    }

    /// <summary>
    /// Обновление существующей сущности.
    /// </summary>
    public virtual async Task UpdateAsync(TUpdateRequest request)
    {
        var entity = await FindEntityToUpdateAsync(request);

        if (entity is not null)
        {
            MapToExistingEntity(entity, request);
            await SaveAsync();
        }
    }

    /// <summary>
    /// Маппинг CreateRequest в новую сущность.
    /// </summary>
    protected abstract Task<TEntity> MapFromCreateRequestAsync(TCreateRequest request);

    /// <summary>
    /// Поиск сущности для обновления.
    /// </summary>
    protected abstract Task<TEntity?> FindEntityToUpdateAsync(TUpdateRequest request);

    /// <summary>
    /// Применение изменений из запроса в существующую сущность.
    /// </summary>
    protected abstract void MapToExistingEntity(TEntity entity, TUpdateRequest request);
}
