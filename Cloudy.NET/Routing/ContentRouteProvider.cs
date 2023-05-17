using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.NET.Routing
{
    public class ContentRouteProvider : IContentRouteProvider
    {
        IEnumerable<ContentRouteDescriptor> Values { get; }

        public ContentRouteProvider(IContentRouteCreator contentRouteCreator)
        {
            Values = contentRouteCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<ContentRouteDescriptor> GetAll()
        {
            return Values;
        }
    }
}
