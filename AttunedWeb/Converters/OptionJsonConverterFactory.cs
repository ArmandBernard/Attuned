using System.Text;
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

        public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var tokenText = Encoding.UTF8.GetString(reader.ValueSpan);

            try
            {
                return Option<T>.Of(JsonSerializer.Deserialize<T>(ref reader, options)!);
            }
            catch (JsonException)
            {
                // The default message is about CLR types and has no field name (the inner call resets the path).
                // A message set here is kept as is, and the serializer adds the property path to the error key.
                var expected = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                var description = expected == typeof(int) ? "a whole number"
                    : expected == typeof(bool) ? "true or false"
                    : $"a valid {expected.Name}";

                throw new JsonException($"\"{tokenText}\" is not {description}.");
            }
        }

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
