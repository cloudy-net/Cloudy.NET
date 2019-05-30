using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public interface IContentSegmentRouter
    {
        IContent RouteContentSegment(string parentId, string segment, string language);
    }
}
