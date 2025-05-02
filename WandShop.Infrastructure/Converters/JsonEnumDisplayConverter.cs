using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WandShop.Infrastructure.Converters
{
    public class JsonEnumDisplayConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value)) throw new JsonException($"Invalid value for enum {typeof(T).Name}");
            var normalized = Regex.Replace(value, @"\s+", "");
            return Enum.TryParse<T>(normalized, ignoreCase: true, out var result)
                ? result
                : throw new JsonException($"Invalid value '{value}' for enum {typeof(T).Name}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Adds a blank space before every upper letter that is not first
            var text = Regex.Replace(value.ToString(), "(\\B[A-Z])", " $1");
            writer.WriteStringValue(text);
        }
    }
}
