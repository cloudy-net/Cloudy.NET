using Cloudy.CMS.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public class ContentSegmentRouter : IContentSegmentRouter
    {
        public object RouteContentSegment(object[] parentKeyValues, string segment, IEnumerable<EntityTypeDescriptor> types)
        {
            throw new NotImplementedException();
        }
    }
}
