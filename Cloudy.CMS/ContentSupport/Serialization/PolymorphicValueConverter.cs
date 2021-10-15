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
            v => ToStringIfNotNull(PolymorphicSerializer.UglyInstance.Serialize(v, typeof(T))),
            v => (T)PolymorphicDeserializer.UglyInstance.Deserialize(JObject.Parse(v), typeof(T))
        ) { }

        static string ToStringIfNotNull(JObject @object) // To avoid the "an expression tree lambda may not contain a null propagating operator" error
        {
            return @object?.ToString();
        }
    }
}
