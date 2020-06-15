using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicSerializer : IPolymorphicSerializer
    {
        ILogger Logger { get; }
        IPolymorphicCandidateProvider PolymorphicCandidateProvider { get; }

        public PolymorphicSerializer(ILogger<PolymorphicSerializer> logger, IPolymorphicCandidateProvider polymorphicCandidateProvider)
        {
            Logger = logger;
            PolymorphicCandidateProvider = polymorphicCandidateProvider;
        }

        public JObject Serialize(object @object, Type type)
        {
            var candidate = PolymorphicCandidateProvider.Get(@object.GetType());

            if (candidate == null)
            {
                Logger.LogInformation($"Found no candidates for polymorphically serializing `{@object.GetType()}` into `{type}`");
                return null;
            }

            var result = new JObject();

            result["Type"] = candidate.Id;
            result["Value"] = JObject.FromObject(@object);

            return result;
        }
    }
}
