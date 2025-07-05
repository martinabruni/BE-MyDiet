using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyDiet.Auth.Business.Mappers
{
    public class ByteArrayBase64Converter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? base64 = reader.GetString();
            return base64 != null ? Convert.FromBase64String(base64) : Array.Empty<byte>();
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            string base64 = Convert.ToBase64String(value);
            writer.WriteStringValue(base64);
        }
    }
}