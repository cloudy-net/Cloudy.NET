using Cloudy.CMS.AspNet.ContentControllerSupport;
using Cloudy.CMS.ContentControllerSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cloudy.CMS.AspNet.Routing
{
    public class ContentRoute : RouteBase
    {
        IContentRouter ContentRouter { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentControllerFinder ContentControllerFinder { get; }

        public ContentRoute(IContentRouter contentRouter, IContentTypeProvider contentTypeRepository, IContentControllerFinder contentControllerFinder)
        {
            ContentRouter = contentRouter;
            ContentTypeRepository = contentTypeRepository;
            ContentControllerFinder = contentControllerFinder;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var segments = (IEnumerable<string>)httpContext.Request.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            IContent content;
            ContentTypeDescriptor contentType;

            content = ContentRouter.RouteContent(segments, "sv");

            if (content == null)
            {
                return null;
            }

            contentType = ContentTypeRepository.Get(content.GetType());

            var match = ContentControllerFinder.FindController(contentType);

            var result = new RouteData(this, new MvcRouteHandler());

            result.Values["controller"] = match.ControllerName;
            result.Values["action"] = match.ControllerAction;

            if (((ContentControllerMatch)match).ParameterName != null)
            {
                result.Values[((ContentControllerMatch)match).ParameterName] = content;
            }

            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
