using System.Text.Json;
using System.Text.Json.Serialization;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Converters;

public class OptionJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter) Activator.CreateInstance(
            typeof(OptionalJsonConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]))!;

    private class OptionalJsonConverter<T> : JsonConverter<Option<T>>
    {
        // let null reach Read, so that null can be told apart from absent
        public override bool HandleNull => true;

        public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            // Reading with the inner converter directly (rather than JsonSerializer.Deserialize) keeps the property
            // path on any error. Null still goes through Deserialize, as the inner converter is not called for it.
            Option<T>.Of(reader.TokenType == JsonTokenType.Null
                ? JsonSerializer.Deserialize<T>(ref reader, options)!
                : ((JsonConverter<T>) options.GetConverter(typeof(T))).Read(ref reader, typeof(T), options)!);

        public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                JsonSerializer.Serialize(writer, value.Value, options);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
