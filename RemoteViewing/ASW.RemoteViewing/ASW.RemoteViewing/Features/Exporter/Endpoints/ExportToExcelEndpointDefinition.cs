using ASW.RemoteViewing.Features.Exporter.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.RemoteViewing.Shared.Security;

namespace ASW.RemoteViewing.Features.Exporter.Endpoints;

public class ExportToExcelEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/api/v1/Remote/Posts/Weighings/Export/Excel", GetWeighingsToExcel)
            .WithName("ExportWeighingsToExcel")
            .WithSummary("Экспортировать взвешивание в Excel")
            .WithDescription("Возвращает Excel файл в байтах")
            .Produces<byte[]>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.Exporter.CanExport);
    }
    internal async Task<IResult> GetWeighingsToExcel(IExportToExcelService service,
        ExportWeighingToExcelRequest request)
    {
        if (request == null)
        { 
            return Results.BadRequest("Request body cannot be null.");
        }
        return Results.File(await service.GetWeighingsToExcel(request), 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WeighingLog.xlsx");
    }
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IExportToExcelService, ExportToExcelService>();
    }
}
