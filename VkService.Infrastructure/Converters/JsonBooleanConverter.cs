using System.Text.Json;
using System.Text.Json.Serialization;

namespace VkService.Client.Abstractions.Infrastructure.Converters;

public class JsonBooleanConverter : JsonConverter<bool>
{
    private const int True = 1;

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number || typeToConvert != typeof(bool))
        {
            throw new InvalidCastException("Invalid casting to bool");
        }

        var number = reader.GetInt16();
        if (number != 0 && number != 1)
        {
            throw new FormatException();
        }

        return number == True;
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}
