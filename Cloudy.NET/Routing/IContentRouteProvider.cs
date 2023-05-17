using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.Routing
{
    public interface IContentRouteProvider
    {
        IEnumerable<ContentRouteDescriptor> GetAll();
    }
}
