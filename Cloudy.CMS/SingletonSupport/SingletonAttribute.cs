using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonAttribute : Attribute
    {
        public IEnumerable<object> KeyValues { get; }

        public SingletonAttribute(params object[] keyValues)
        {
            if (keyValues.Length == 0)
            {
                throw new Exception($"Primary key must be provided when using [Singleton(...)]");
            }

            KeyValues = keyValues.ToList().AsReadOnly();
        }
    }
}
