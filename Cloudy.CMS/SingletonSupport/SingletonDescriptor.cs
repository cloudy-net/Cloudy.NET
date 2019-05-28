using System;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonDescriptor
    {
        public string Id { get; }
        public Type Type { get; }

        public SingletonDescriptor(string id, Type type)
        {
            Id = id;
            Type = type;
        }
    }
}