using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public interface IContentSegmentRouter
    {
        IContent RouteContentSegment(string parentId, string segment, IEnumerable<ContentTypeDescriptor> types, string language);
    }
}
