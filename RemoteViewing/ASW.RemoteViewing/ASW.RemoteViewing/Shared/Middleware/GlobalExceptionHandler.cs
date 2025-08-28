using ASW.RemoteViewing.Shared.Responses;
using ASW.Shared.Extentions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ASW.RemoteViewing.Shared.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    { 
        var statusCode = exception is ValidationException ? 400 : 500;
        var problem = new ProblemDetails
        {
            Title = "Ошибка",
            Detail = statusCode == 400 ? exception.Message : "Неизвестная ошибка на сервере",
            Status = statusCode,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}