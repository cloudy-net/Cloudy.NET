using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    public class PolymorphicFormConverter : JsonConverter
    {
        ILogger Logger { get; }
        IPolymorphicCandidateProvider PolymorphicCandidateProvider { get; }
        IHumanizer Humanizer { get; }

        JsonSerializer JsonWebSerializer { get; }

        public PolymorphicFormConverter(ILogger<PolymorphicFormConverter> logger, IPolymorphicCandidateProvider polymorphicCandidateProvider, IHumanizer humanizer)
        {
            Logger = logger;
            PolymorphicCandidateProvider = polymorphicCandidateProvider;
            Humanizer = humanizer;
            JsonWebSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = (sender, args) =>
                {
                    Logger.LogInformation(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
            });
        }

        public override bool CanConvert(Type objectType)
        {
            foreach(var candidate in PolymorphicCandidateProvider.GetAll())
            {
                foreach(var @interface in candidate.Type.GetInterfaces())
                {
                    if (@interface.IsAssignableFrom(objectType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!objectType.IsInterface)
            {
                return JObject.Load(reader).ToObject(objectType);
            }

            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType != JsonToken.StartObject)
            {
                Logger.LogInformation("Polymorphic properties must be JSON objects of the schema: { type: ..., value: {...} }");
                return null;
            }

            var container = JObject.Load(reader);

            var typeId = container.Value<string>("type");

            if(typeId == null)
            {
                Logger.LogInformation("Container object is missing the required type property");
                return null;
            }

            var candidate = PolymorphicCandidateProvider.Get(typeId);

            if (candidate == null)
            {
                Logger.LogInformation($"No such type: {typeId}");
                return null;
            }

            var value = container.Value<JObject>("value");

            return value.ToObject(candidate.Type);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var o = new JObject();

            var candidate = PolymorphicCandidateProvider.Get(value.GetType());

            if(candidate == null)
            {
                throw new Exception($"Value (of type {value.GetType()}) must be a [ContentType] when saving to an interface type property for polymorphic serialization to work");
            }

            o["type"] = candidate.Id;
            o["name"] = candidate.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? Humanizer.Humanize(candidate.Type.Name);
            o["value"] = JObject.FromObject(value, JsonWebSerializer);

            o.WriteTo(writer);
        }
    }
}