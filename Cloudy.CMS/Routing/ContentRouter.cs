using Cloudy.CMS.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport;

namespace Cloudy.CMS.Routing
{
    public class ContentRouter : IContentRouter
    {
        IContextCreator ContextCreator { get; }
        IRootContentRouter RootContentRouter { get; }
        IRoutableRootContentProvider RoutableRootContentProvider { get; }

        public ContentRouter(IContextCreator contextCreator, IRootContentRouter rootContentRouter, IRoutableRootContentProvider routableRootContentProvider)
        {
            ContextCreator = contextCreator;
            RootContentRouter = rootContentRouter;
            RoutableRootContentProvider = routableRootContentProvider;
        }

        public async Task<object> RouteContentAsync(IEnumerable<string> segments, IEnumerable<EntityTypeDescriptor> types)
        {
            segments = segments.Where(s => !string.IsNullOrEmpty(s)).ToList().AsReadOnly();

            if(segments.Count() == 1 || segments.Count() == 0)
            {
                var segment = segments.SingleOrDefault();

                foreach(var type in types.Where(t => t.IsRoutable && !t.IsHierarchical))
                {
                    var dbSet = ContextCreator.CreateFor(type.Type).GetDbSet(type.Type);

                    var result = ((IQueryable)dbSet).Cast<IRoutable>().Where(r => r.UrlSegment == segment).FirstOrDefault();

                    if(result != null)
                    {
                        return result;
                    }
                }

                return null;
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
