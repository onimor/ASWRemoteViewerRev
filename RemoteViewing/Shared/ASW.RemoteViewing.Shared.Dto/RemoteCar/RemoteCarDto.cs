namespace ASW.RemoteViewing.Shared.Dto.RemoteCar;

public class RemoteCarDto
{
    public Guid Id { get; set; }
    public string GovernmentNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid? DriverId { get; set; }
    public string? DriverName { get; set; } = string.Empty;
    public Guid? GoodsId { get; set; }
    public string? GoodsName { get; set; } = string.Empty;
    public Guid? TrailerId { get; set; }
    public string? TrailerName { get; set; } = string.Empty;
    public Guid? SenderId { get; set; }
    public string? SenderName { get; set; } = string.Empty;
    public Guid? RecipientId { get; set; }
    public string? RecipientName { get; set; } = string.Empty;
    public Guid? CarrierId { get; set; }
    public string? CarrierName { get; set; } = string.Empty;
    public Guid? PayerId { get; set; }
    public string? PayerName { get; set; } = string.Empty;
    public string RFID { get; set; } = string.Empty;
}
