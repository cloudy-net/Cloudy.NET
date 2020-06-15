using Newtonsoft.Json.Linq;
using System;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IPolymorphicDeserializer
    {
        object Deserialize(JObject @object, Type type);
    }
}