using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cloudy.NET.UI.Serialization
{
    internal class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string SerializationFormat = "HH:mm:ss.fff";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return TimeOnly.Parse(value!);
        }

        public override void Write(
            Utf8JsonWriter writer,
            TimeOnly value,
            JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(SerializationFormat));
    }
}
