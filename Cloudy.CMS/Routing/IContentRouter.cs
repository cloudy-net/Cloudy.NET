using Cloudy.CMS.ContentSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouter
    {
        IContent RouteContent(IEnumerable<string> segments, string language);
    }
}