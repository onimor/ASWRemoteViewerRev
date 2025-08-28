namespace ASW.RemoteViewing.Infrastructure.Data.Base;

public interface IEntity<TId>
{
    TId Id { get; set; }
}
