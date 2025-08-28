namespace ASW.RemoteViewing.Shared.Dto.RemoteCounterparty;

public class RemoteCounterpartyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LegalAddress { get; set; } = string.Empty;
    public string ActualAddress { get; set; } = string.Empty;
    public string Contacts { get; set; } = string.Empty;
    public string INN { get; set; } = string.Empty;
    public string KPP { get; set; } = string.Empty;
    public string OGRN { get; set; } = string.Empty;
    public bool IsWarehouse { get; set; }
}
