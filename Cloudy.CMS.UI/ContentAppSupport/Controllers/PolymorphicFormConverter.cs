using Cloudy.CMS.UI.FormSupport;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    public class PolymorphicFormConverter : JsonConverter
    {
        ILogger Logger { get; }
        IFormProvider FormProvider { get; }

        public PolymorphicFormConverter(ILogger<PolymorphicFormConverter> logger, IFormProvider formProvider)
        {
            Logger = logger;
            FormProvider = formProvider;
        }

        public override bool CanConvert(Type objectType)
        {
            foreach(var form in FormProvider.GetAll())
            {
                foreach(var @interface in form.Type.GetInterfaces())
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
                return JObject.Load(reader).ToObject(objectType, serializer);
            }

            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType != JsonToken.StartObject)
            {
                Logger.LogInformation("Polymorphic forms must be JObjects of the schema: { formId: ..., value: {...} }");
                return null;
            }

            var container = JObject.Load(reader);

            var formId = container.Value<string>("formId");

            if(formId == null)
            {
                Logger.LogInformation("Container object is missing the required formId property");
                return null;
            }

            var form = FormProvider.Get(formId);

            if (form == null)
            {
                Logger.LogInformation($"No such form: {formId}");
                return null;
            }

            var value = container.Value<JObject>("value");

            return value.ToObject(form.Type, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var o = new JObject();

            var form = FormProvider.Get(value.GetType());

            if(form == null)
            {
                throw new Exception($"Value of type ({value.GetType()}) must be a form when saving to an interface type property (polymorphic form)");
            }

            o["FormId"] = form.Id;
            o["Value"] = JObject.FromObject(value);

            o.WriteTo(writer);
        }
    }
}