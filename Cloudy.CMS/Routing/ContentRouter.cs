using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
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

        public IContent RouteContent(IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types)
        {
            foreach(var root in RoutableRootContentProvider.GetAll())
            {
                if(types.Any() && !types.Any(t => t.Type == root.GetType()))
                {
                    continue;
                }

                var result = RootContentRouter.Route(root, segments, types);

                if(result == null)
                {
                    continue;
                }

                return result;
            }

            return null;
        }
    }
}
