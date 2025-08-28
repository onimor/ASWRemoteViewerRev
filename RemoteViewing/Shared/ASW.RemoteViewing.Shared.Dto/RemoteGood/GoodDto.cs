namespace ASW.RemoteViewing.Shared.Dto.RemoteGood;

public class RemoteGoodDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string VendorCode { get; set; } = string.Empty;
}
