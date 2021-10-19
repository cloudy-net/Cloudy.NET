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
        IContextProvider ContextProvider { get; }
        IRootContentRouter RootContentRouter { get; }
        IRoutableRootContentProvider RoutableRootContentProvider { get; }

        public ContentRouter(IContextProvider contextProvider, IRootContentRouter rootContentRouter, IRoutableRootContentProvider routableRootContentProvider)
        {
            ContextProvider = contextProvider;
            RootContentRouter = rootContentRouter;
            RoutableRootContentProvider = routableRootContentProvider;
        }

        public async Task<object> RouteContentAsync(IEnumerable<string> segments, IEnumerable<ContentTypeDescriptor> types)
        {
            if(segments.Count() == 1 || segments.Count() == 0)
            {
                var segment = segments.SingleOrDefault();

                foreach(var type in types)
                {
                    var dbSet = ContextProvider.GetFor(type.Type).GetDbSet(type.Type);

                    var result = ((IQueryable)dbSet.DbSet).Cast<IRoutable>().Where(r => r.UrlSegment == segment).FirstOrDefault();

                    if(result != null)
                    {
                        return result;
                    }
                }
            }

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
