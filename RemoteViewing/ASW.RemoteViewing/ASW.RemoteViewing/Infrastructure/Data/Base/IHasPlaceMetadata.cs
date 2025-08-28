namespace ASW.RemoteViewing.Infrastructure.Data.Base;

public interface IHasPlaceMetadata
{
    Guid PlaceId { get; set; }
    string PlaceName { get; set; }
}
