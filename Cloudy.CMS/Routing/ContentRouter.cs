using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.Routing
{
    public class ContentRouter : IContentRouter
    {
        IContentSegmentRouter ContentSegmentRouter { get; }

        public ContentRouter(IContentSegmentRouter contentSegmentRouter)
        {
            ContentSegmentRouter = contentSegmentRouter;
        }

        public IContent RouteContent(IEnumerable<string> segments, string language)
        {
            IContent page = null;

            if (!segments.Any())
            {
                return ContentSegmentRouter.RouteContentSegment(null, null, language);
            }

            while (segments.Any())
            {
                page = ContentSegmentRouter.RouteContentSegment(segments.First(), page?.Id, language);

                if (page == null)
                {
                    return null;
                }

                segments = segments.Skip(1);
            }

            return page;
        }
    }
}
