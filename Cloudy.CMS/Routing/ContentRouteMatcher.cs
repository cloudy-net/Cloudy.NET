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
            var result = new List<ContentRouteDescriptor>();

            foreach(var contentRoute in ContentRouteProvider.GetAll())
            {
                if(!contentRoute.ContentTypes.Any(c => c.Id == contentType.Id))
                {
                    continue;
                }

                result.Add(contentRoute);
            }

            return result.AsReadOnly();
        }
    }
}
