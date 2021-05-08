using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouter
    {
        object RouteContent(IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types);
    }
}