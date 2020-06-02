using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Integrations.JsonDotNet;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class SerializerRegistry : IBsonSerializerRegistry
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            return (IBsonSerializer)typeof(SerializerRegistry).GetMethods().Where(m => m.Name == nameof(GetSerializer) && m.ContainsGenericParameters).Single().MakeGenericMethod(type).Invoke(this, new object[] { });
        }

        public IBsonSerializer<T> GetSerializer<T>()
        {
            return new JsonSerializerAdapter<T>();
        }
    }
}