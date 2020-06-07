using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonAttribute : Attribute
    {
        public string Id { get; }

        public SingletonAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Id must be provided when using [Singleton(...)]. How about {Guid.NewGuid()} ?");
            }

            Id = id;
        }
    }
}
