using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonDescriptor
    {
        public string ContentTypeId { get; }
        public Type Type { get; }

        public SingletonDescriptor(string contentTypeId, Type type)
        {
            ContentTypeId = contentTypeId;
            Type = type;
        }
    }
}