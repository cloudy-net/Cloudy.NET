using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class PolymorphicValueConverter<T> : ValueConverter<T, string> where T: class
    {
        public PolymorphicValueConverter() : base(
            value => Serialize(value),
            value => Deserialize(value)
        )
        { }

        static T Deserialize(string value)
        {
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                var type = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(IEnumerable<>) ? typeof(T).GetGenericArguments()[0] : typeof(T).GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)).Select(t => t.GetGenericArguments()[0]).First();
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

                foreach (var element in JArray.Parse(value))
                {
                    list.Add(PolymorphicDeserializer.UglyInstance.Deserialize((JObject)element, typeof(T)));
                }

                return (T)list;
            }

            return (T)PolymorphicDeserializer.UglyInstance.Deserialize(JObject.Parse(value), typeof(T));
        }

        static string Serialize(T value)
        {
            if(value == null)
            {
                return null;
            }

            if(value is IEnumerable)
            {
                var array = new JArray();

                foreach(var element in (IEnumerable)value)
                {
                    array.Add(PolymorphicSerializer.UglyInstance.Serialize(element, typeof(T)));
                }

                return array.ToString();
            }

            return PolymorphicSerializer.UglyInstance.Serialize(value, typeof(T)).ToString();
        }
    }
}
