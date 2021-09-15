using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public class ContentSegmentRouter : IContentSegmentRouter
    {
        public object RouteContentSegment(object[] parentKeyValues, string segment, IEnumerable<ContentTypeDescriptor> types)
        {
            throw new NotImplementedException();
        }
    }
}
