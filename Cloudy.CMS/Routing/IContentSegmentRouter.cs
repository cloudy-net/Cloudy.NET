using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public interface IContentSegmentRouter
    {
        object RouteContentSegment(object[] parentKeyValues, string segment, IEnumerable<ContentTypeDescriptor> types);
    }
}
