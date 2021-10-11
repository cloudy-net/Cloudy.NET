using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicValueConverter<T> : ValueConverter<T, string> where T: class
    {
        public PolymorphicValueConverter() : base(
            v => PolymorphicSerializer.UglyInstance.Serialize(v, typeof(T)).ToString(),
            v => (T)PolymorphicDeserializer.UglyInstance.Deserialize(JObject.Parse(v), typeof(T))
        ) { }
    }
}
