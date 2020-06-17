using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicDeserializer : IPolymorphicDeserializer
    {
        ILogger Logger { get; }
        IPolymorphicCandidateProvider PolymorphicCandidateProvider { get; }

        JsonSerializer JsonSerializer { get; }

        public PolymorphicDeserializer(ILogger<PolymorphicDeserializer> logger, IPolymorphicCandidateProvider polymorphicCandidateProvider)
        {
            Logger = logger;
            PolymorphicCandidateProvider = polymorphicCandidateProvider;
            JsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Error = (sender, args) =>
                {
                    Logger.LogInformation(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
            });
        }

        public object Deserialize(JObject @object, Type type)
        {
            var targetTypeId = @object.Value<string>("Type");

            if(targetTypeId == null)
            {
                Logger.LogInformation($"Object `{@object.ToString(Newtonsoft.Json.Formatting.None)}` needs property `type` in order to be polymorphically deserialized into `{type}`");
                return null;
            }

            var candidate = PolymorphicCandidateProvider.Get(targetTypeId);

            if(candidate == null)
            {
                Logger.LogInformation($"Found no candidates for polymorphically deserializing `{@object.ToString(Newtonsoft.Json.Formatting.None)}` into `{type}`");
                return null;
            }

            return @object.Value<JObject>("Value").ToObject(candidate.Type, JsonSerializer);
        }
    }
}
