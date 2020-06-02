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
        IRootContentRouter RootContentRouter { get; }
        IRoutableRootContentProvider RoutableRootContentProvider { get; }

        public ContentRouter(IRootContentRouter rootContentRouter, IRoutableRootContentProvider routableRootContentProvider)
        {
            RootContentRouter = rootContentRouter;
            RoutableRootContentProvider = routableRootContentProvider;
        }

        public IContent RouteContent(IEnumerable<string> segments, string language)
        {
            foreach(var root in RoutableRootContentProvider.GetAll())
            {
                var result = RootContentRouter.Route(root, segments, language);

                if(result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
