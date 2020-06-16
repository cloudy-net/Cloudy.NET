using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;

namespace Cloudy.CMS.Routing
{
    public class RootContentRouter : IRootContentRouter
    {
        IContentSegmentRouter ContentSegmentRouter { get; }

        public RootContentRouter(IContentSegmentRouter contentSegmentRouter)
        {
            ContentSegmentRouter = contentSegmentRouter;
        }

        public IContent Route(IContent root, IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types, string language)
        {
            if (!segments.Any())
            {
                if (((IRoutable)root).UrlSegment == null && (!types.Any() || types.Any(t => t.Type == root.GetType())))
                {
                    return root;
                }
                
                return null;
            }

            if (((IRoutable)root).UrlSegment != null && ((IRoutable)root).UrlSegment.Equals(segments.First()))
            {
                segments = segments.Skip(1);
            }

            IContent content = root;

            while (segments.Any())
            {
                content = ContentSegmentRouter.RouteContentSegment(content?.Id, segments.First(), types, language);

                if (content == null)
                {
                    return null;
                }

                if(types.Any() && !types.Any(t => t.Type == root.GetType()))
                {
                    return null;
                }

                segments = segments.Skip(1);
            }

            return content;
        }
    }
}
