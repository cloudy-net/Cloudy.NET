using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.ContentSupport;

namespace Cloudy.CMS.Routing
{
    public class RootContentRouter : IRootContentRouter
    {
        IContentSegmentRouter ContentSegmentRouter { get; }

        public RootContentRouter(IContentSegmentRouter contentSegmentRouter)
        {
            ContentSegmentRouter = contentSegmentRouter;
        }

        public IContent Route(IContent root, IEnumerable<string> segments, string language)
        {
            IContent content = root;

            if (((IRoutable)content).UrlSegment != null && ((IRoutable)content).UrlSegment.Equals(segments.First()))
            {
                segments = segments.Skip(1);
            }

            while (segments.Any())
            {
                content = ContentSegmentRouter.RouteContentSegment(content?.Id, segments.First(), language);

                if (content == null)
                {
                    return null;
                }

                segments = segments.Skip(1);
            }

            return content;
        }
    }
}
