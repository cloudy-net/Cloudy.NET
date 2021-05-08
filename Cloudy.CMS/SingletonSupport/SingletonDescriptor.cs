using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonDescriptor
    {
        public IEnumerable<object> KeyValues { get; }
        public string ContentTypeId { get; }
        public Type Type { get; }

        public SingletonDescriptor(IEnumerable<object> keyValues, string contentTypeId, Type type)
        {
            KeyValues = keyValues.ToList().AsReadOnly();
            ContentTypeId = contentTypeId;
            Type = type;
        }
    }
}