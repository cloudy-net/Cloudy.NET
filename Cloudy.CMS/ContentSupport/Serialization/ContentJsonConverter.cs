using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentJsonConverter : JsonConverter<object>
    {
        public static ContentJsonConverter UglyInstance { get; set; }

        IContentTypeProvider ContentTypeProvider { get; }

        public ContentJsonConverter(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return ContentTypeProvider.Get(typeToConvert) != null || ContentTypeProvider.GetAll().Any(t => typeToConvert.IsAssignableFrom(t.Type));
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return null;
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                return null;
            }

            string propertyName = reader.GetString();
            if (propertyName != "__Cloudy_ContentType")
            {
                return null;
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                return null;
            }

            var contentTypeId = reader.GetString();
            var contentType = ContentTypeProvider.Get(contentTypeId);

            if (contentType == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize(ref reader, contentType.Type, options);
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var contentType = ContentTypeProvider.Get(value.GetType());

            if (contentType == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("Type", contentType.Id);
            writer.WritePropertyName("Value");

            writer.WriteStartObject();
            var properties = contentType.Type.GetProperties().Where(p => !p.GetIndexParameters().Any() && p.GetGetMethod() != null && !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)));
            foreach (var property in properties)
            {
                JsonSerializer.Serialize(writer, property.GetValue(value), property.PropertyType, options);
            }
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
