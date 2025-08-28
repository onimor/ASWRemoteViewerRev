using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASW.RemoteViewing.Client.Infrastructure.Http;

public class DateTimeLocalJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var parsed = DateTime.Parse(reader.GetString()!, null, DateTimeStyles.RoundtripKind);
        return parsed.ToLocalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utc = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        writer.WriteStringValue(utc.ToString("o")); // ISO 8601
    }
}
