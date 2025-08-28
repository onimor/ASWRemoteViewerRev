namespace ASW.RemoteViewing.Shared.Dto.ExportToExcel;

public class ExportToExcelFiltersDto
{
    public Guid? GoodId { get; set; }
    public Guid? SenderId { get; set; }
    public Guid? RecipientId { get; set; }
    public Guid? CarrierId { get; set; }
    public Guid? PayerId { get; set; }
}
