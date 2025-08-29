using ASW.RemoteViewing.Features.RemoteWeighing.Services;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Extension;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ASW.RemoteViewing.Features.Exporter.Services;

public class ExportToExcelService : IExportToExcelService
{
    private readonly IRemoteWeighingService _weighingService;
    public ExportToExcelService(IRemoteWeighingService weighingService)
    {
        _weighingService = weighingService;
    }
    public async Task<byte[]> GetWeighingsToExcel(ExportWeighingToExcelRequest weighingExportToExcelRequest)
    {
        ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

        var weighing = await _weighingService.GetByDateAsync(new RemoteWeightByDateRequest
        {
            DateStart = weighingExportToExcelRequest.DateStart,
            DateEnd = weighingExportToExcelRequest.DateEnd
        });

        var filters = weighingExportToExcelRequest.Filters;

        weighing = weighing?
            .Where(x => x.IsRemoved == false)
            .Where(x => weighingExportToExcelRequest.WeighingStage == "Все" ||
                       weighingExportToExcelRequest.WeighingStage == "Одно взвешивание" && x.IsFormed == false ||
                       weighingExportToExcelRequest.WeighingStage == "Оформленные" && x.IsFormed == true)
            .Where(x => weighingExportToExcelRequest.PostName == "Все" || x.PostId == weighingExportToExcelRequest.PostId)
            .Where(x => filters == null ||
                       (filters.GoodId.HasValue == false || filters.GoodId == Guid.Empty || x.GoodsId == filters.GoodId) &&
                       (filters.SenderId.HasValue == false || filters.SenderId == Guid.Empty || x.SenderId == filters.SenderId) &&
                       (filters.RecipientId.HasValue == false || filters.RecipientId == Guid.Empty || x.RecipientId == filters.RecipientId) &&
                       (filters.CarrierId.HasValue == false || filters.CarrierId == Guid.Empty || x.CarrierId == filters.CarrierId) &&
                       (filters.PayerId.HasValue == false || filters.PayerId == Guid.Empty || x.PayerId == filters.PayerId))
            .ToList();

        if (weighing is null && weighing?.Count <= 0)
            throw new ValidationException("Нет данных для выгрузки");
        var package = new ExcelPackage();
        using var worksheet = package.Workbook.Worksheets.Add($"C {weighingExportToExcelRequest.DateStart?.ToString("dd.MM.yyyy")} ПО {weighingExportToExcelRequest.DateEnd?.ToString("dd.MM.yyyy")}");
        // заголовки
        worksheet.Cells[1, 1].Value = "Номер";
        worksheet.Cells[1, 2].Value = "Пост";
        worksheet.Cells[1, 3].Value = "Дата тара";
        worksheet.Cells[1, 4].Value = "Тара";
        worksheet.Cells[1, 5].Value = "Дата брутто";
        worksheet.Cells[1, 6].Value = "Брутто";
        worksheet.Cells[1, 7].Value = "Нетто";
        worksheet.Cells[1, 8].Value = "ТС";
        worksheet.Cells[1, 9].Value = "Водитель";
        worksheet.Cells[1, 10].Value = "Груз";
        worksheet.Cells[1, 11].Value = "Прицеп";
        worksheet.Cells[1, 12].Value = "Отправитель";
        worksheet.Cells[1, 13].Value = "Получатель";
        worksheet.Cells[1, 14].Value = "Перевозчик";
        worksheet.Cells[1, 15].Value = "Плательщик";
        worksheet.Cells[1, 16].Value = "RFID";
        worksheet.Cells[1, 17].Value = "Способ доставки"; 
        worksheet.Cells[1, 18].Value = "Пользователь";

        for (int i = 1; i < 19; i++)
        {
            using (ExcelRange rng = worksheet.Cells[1, i, 2, i])
            { 
                rng.AutoFitColumns();
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                rng.Merge = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(200, 204, 204, 255);
            }

        }
        for (int i = 0; i < weighing?.Count; i++) 
        {
            worksheet.Cells[3 + i, 1].Value = weighing?[i].Number;
            worksheet.Cells[3 + i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[3 + i, 2].Value = weighing?[i].PostName;
            worksheet.Cells[3 + i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 3].Value = weighing?[i].DateTara;
            worksheet.Cells[3 + i, 3].Style.Numberformat.Format = "dd.MM.yyyy HH:mm:ss";
            worksheet.Cells[3 + i, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 4].Value = weighing?[i].Tara;
            worksheet.Cells[3 + i, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //worksheet.Cells[3 + i, 4].Style.Numberformat.Format = "0.000";
            worksheet.Cells[3 + i, 5].Value = weighing?[i].DateBrutto;
            worksheet.Cells[3 + i, 5].Style.Numberformat.Format = "dd.MM.yyyy HH:mm:ss";
            worksheet.Cells[3 + i, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 6].Value = weighing?[i].Brutto;
            worksheet.Cells[3 + i, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //worksheet.Cells[3 + i, 6].Style.Numberformat.Format = "0.000";
            worksheet.Cells[2 + i, 7].Value = weighing?[i].Netto;
            worksheet.Cells[2 + i, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //worksheet.Cells[3 + i, 7].Style.Numberformat.Format = "0.000";
            worksheet.Cells[3 + i, 8].Value = weighing?[i].CarName; 
            worksheet.Cells[3 + i, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 9].Value = weighing?[i].DriverName; 
            worksheet.Cells[3 + i, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 10].Value = weighing?[i].GoodsName; 
            worksheet.Cells[3 + i, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 11].Value = weighing?[i].TrailerName; 
            worksheet.Cells[3 + i, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 12].Value = weighing?[i].SenderName; 
            worksheet.Cells[3 + i, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 13].Value = weighing?[i].RecipientName; 
            worksheet.Cells[3 + i, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 14].Value = weighing?[i].CarrierName; 
            worksheet.Cells[3 + i, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 15].Value = weighing?[i].PayerName; 
            worksheet.Cells[3 + i, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 16].Value = weighing?[i].RFID; 
            worksheet.Cells[3 + i, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 17].Value = weighing?[i].DeliveryMethod; 
            worksheet.Cells[3 + i, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3 + i, 18].Value = weighing?[i].AddedUserName;
            worksheet.Cells[3 + i, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            using (ExcelRange rng = worksheet.Cells[3, 1, weighing.Count +2, 18])
            {
                rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            }  
        }
        for (int i = 1; i < 17; i++)
        {
            worksheet.Columns[i].AutoFit();
        }
        
        worksheet.View.FreezePanes(3, 1);
        return await package.GetAsByteArrayAsync();
    }
}
