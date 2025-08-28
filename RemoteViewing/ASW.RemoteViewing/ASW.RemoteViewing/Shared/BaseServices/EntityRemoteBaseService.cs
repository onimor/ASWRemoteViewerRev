using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Infrastructure.Data.Base; 
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Shared.BaseServices;

/// <summary>
/// Базовый универсальный сервис для сущностей, привязанных к месту (точке взвешивания) и поддерживающий Create/Update.
/// </summary>
public abstract class EntityRemoteBaseService<TContext, TEntity, TDto, TCreateRequest, TUpdateRequest>
    : EntityServiceBase<TContext, TEntity, TDto, Guid>
    where TContext : DbContext
    where TEntity : class, IEntity<Guid>, ISoftRemovable, IHasPlaceMetadata, new()
{
    protected readonly ICurrentPlaceUserProvider _placeUserProvider;

    protected EntityRemoteBaseService(
        TContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, mapper)
    {
        _placeUserProvider = placeUserProvider;
    }

    /// <summary>
    /// Создание новой сущности с метаданными о пользователе места.
    /// </summary>
    public virtual async Task<TCreateRequest> CreateAsync(TCreateRequest request)
    {
        var entity = MapFromCreateRequest(request);
        await SetMetadataAsync(entity);
        await _dbSet.AddAsync(entity);
        await SaveAsync();

        return request;
    }

    /// <summary>
    /// Обновление существующей сущности или создание, если не найдена.
    /// </summary>
    public virtual async Task UpdateAsync(TUpdateRequest request)
    {
        var entity = await FindEntityToUpdateAsync(request);

        if (entity is null)
        {
            entity = MapFromUpdateRequest(request); 
            await SetMetadataAsync(entity);
            await _dbSet.AddAsync(entity);
        }
        else
        {
            MapToExistingEntity(entity, request);
        }

        await SaveAsync();
    }

    /// <summary>
    /// Отображение CreateRequest на сущность.
    /// </summary>
    protected abstract TEntity MapFromCreateRequest(TCreateRequest request);

    /// <summary>
    /// Отображение UpdateRequest на новую сущность (если не найдена).
    /// </summary>
    protected abstract TEntity MapFromUpdateRequest(TUpdateRequest request);

    /// <summary>
    /// Отображение UpdateRequest в уже существующую сущность.
    /// </summary>
    protected abstract void MapToExistingEntity(TEntity entity, TUpdateRequest request);

    /// <summary>
    /// Поиск сущности для обновления. Реализация зависит от конкретного контекста.
    /// </summary>
    protected abstract Task<TEntity?> FindEntityToUpdateAsync(TUpdateRequest request);

    /// <summary>
    /// Устанавливает PlaceId и PlaceName из текущего пользователя.
    /// </summary>
    protected virtual async Task SetMetadataAsync(TEntity entity)
    {
        var user = await _placeUserProvider.GetCurrentUserAsync();
        entity.PlaceId = user.Id;
        entity.PlaceName = user.Name;
    }
}
