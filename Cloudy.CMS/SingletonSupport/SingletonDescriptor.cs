using System;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonDescriptor
    {
        public string Id { get; }
        public string ContentTypeId { get; }
        public Type Type { get; }

        public SingletonDescriptor(string id, string contentTypeId, Type type)
        {
            Id = id;
            ContentTypeId = contentTypeId;
            Type = type;
        }
    }
}