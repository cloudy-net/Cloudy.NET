using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IRootContentRouter
    {
        object Route(object root, IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types);
    }
}