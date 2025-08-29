using ASW.Shared.Extension;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASW.RemoteViewing.Shared.Utilities.JsonConverters;

public class StrictIso8601DateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] StrictIso8601UtcFormats =
    {
        // Строгие форматы с 'Z' (UTC)
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "yyyy-MM-ddTHH:mm:ss.ffffffZ",
        "yyyy-MM-ddTHH:mm:ss.fffffffZ",

        // Строгие форматы с offset (±HH:mm)
        "yyyy-MM-ddTHH:mm:sszzz",
        "yyyy-MM-ddTHH:mm:ss.fffzzz",
        "yyyy-MM-ddTHH:mm:ss.ffffffzzz",
        "yyyy-MM-ddTHH:mm:ss.fffffffzzz"
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (string.IsNullOrEmpty(str) ||
            !DateTimeOffset.TryParseExact(
                str,
                StrictIso8601UtcFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dto))
        {
            throw new ValidationException($"Invalid ISO 8601 date format. Value: '{str}'");
        }

        return dto.UtcDateTime; // DateTime в UTC
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Преобразуем к UTC и выводим в формате ISO 8601 с Z
        var utcValue = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        writer.WriteStringValue(utcValue.ToString("o"));
    }
}

 
