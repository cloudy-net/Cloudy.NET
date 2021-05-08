using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IRootContentRouter
    {
        IContent Route(IContent root, IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types);
    }
}