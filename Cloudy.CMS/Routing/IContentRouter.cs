using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouter
    {
        IContent RouteContent(IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types, string language);
    }
}