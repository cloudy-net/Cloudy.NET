using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouteProvider
    {
        IEnumerable<ContentRouteDescriptor> GetAll();
    }
}
