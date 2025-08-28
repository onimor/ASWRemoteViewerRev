using System.Globalization;
using System.Reflection;

namespace ASW.RemoteViewing.Shared.Utilities.ModelBinding;

public readonly struct StrictIso8601UtcDateTime
{
    public DateTime UtcDateTime { get; }

    private static readonly string[] StrictIso8601UtcFormats =
    {
    "yyyy-MM-ddTHH:mm:ssZ",
    "yyyy-MM-ddTHH:mm:ss.fffZ",
    "yyyy-MM-ddTHH:mm:ss.ffffffZ",
    "yyyy-MM-ddTHH:mm:ss.fffffffZ",
    "yyyy-MM-ddTHH:mm:sszzz",
    "yyyy-MM-ddTHH:mm:ss.fffzzz",
    "yyyy-MM-ddTHH:mm:ss.ffffffzzz",
    "yyyy-MM-ddTHH:mm:ss.fffffffzzz", 

};

    public StrictIso8601UtcDateTime(DateTime utcDateTime)
    {
        UtcDateTime = utcDateTime;
    }

    public static bool TryParse(string? str, out StrictIso8601UtcDateTime result)
    {  
        if (str != null && DateTimeOffset.TryParseExact(
            str,
            StrictIso8601UtcFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var dto))
        {
            result = new StrictIso8601UtcDateTime(dto.UtcDateTime);
            return true;
        }
        result = default; 
        return false;
    }

    public static ValueTask<StrictIso8601UtcDateTime?> BindAsync(HttpContext context, ParameterInfo parameterInfo)
     => BindAsync(context, parameterInfo.Name);

    private static ValueTask<StrictIso8601UtcDateTime?> BindAsync(HttpContext context, string? parameterName)
    {
        if (string.IsNullOrEmpty(parameterName))
            return ValueTask.FromResult<StrictIso8601UtcDateTime?>(null);

        var value = context.Request.Query[parameterName].ToString()
                    ?? context.Request.RouteValues[parameterName]?.ToString();

        return ValueTask.FromResult<StrictIso8601UtcDateTime?>(
            TryParse(value, out var parsed) ? parsed : null
        );
    }
}
