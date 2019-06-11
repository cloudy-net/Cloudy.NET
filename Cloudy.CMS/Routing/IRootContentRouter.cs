using Cloudy.CMS.ContentSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IRootContentRouter
    {
        IContent Route(IContent root, IEnumerable<string> segments, string language);
    }
}