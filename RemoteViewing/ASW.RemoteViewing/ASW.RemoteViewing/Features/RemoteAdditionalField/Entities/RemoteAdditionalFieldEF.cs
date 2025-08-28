using ASW.RemoteViewing.Infrastructure.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace ASW.RemoteViewing.Features.RemoteAdditionalField.Entities;

public class RemoteAdditionalFieldEF : IEntity<Guid>, ISoftRemovable, IHasPlaceMetadata
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid AdditionalFieldId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSavedLastValue { get; set; }
    public string DefaultValue { get; set; } = string.Empty;
    public bool IsRemoved { get; set; }
}
