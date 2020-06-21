using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteMatcher : IContentRouteMatcher
    {
        public IContentRouteProvider ContentRouteProvider { get; }

        public ContentRouteMatcher(IContentRouteProvider contentRouteProvider)
        {
            ContentRouteProvider = contentRouteProvider;
        }

        public IEnumerable<ContentRouteDescriptor> GetFor(ContentTypeDescriptor contentType)
        {
            throw new NotImplementedException();
        }
    }
}
