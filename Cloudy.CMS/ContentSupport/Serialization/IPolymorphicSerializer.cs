using Newtonsoft.Json.Linq;
using System;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IPolymorphicSerializer
    {
        JObject Serialize(object element, Type type);
    }
}