using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicDeserializer : IPolymorphicDeserializer
    {
        ILogger Logger { get; }
        IPolymorphicCandidateProvider PolymorphicCandidateProvider { get; }

        public PolymorphicDeserializer(ILogger<PolymorphicDeserializer> logger, IPolymorphicCandidateProvider polymorphicCandidateProvider)
        {
            Logger = logger;
            PolymorphicCandidateProvider = polymorphicCandidateProvider;
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

            return @object.Value<JObject>("Value").ToObject(candidate.Type);
        }
    }
}
