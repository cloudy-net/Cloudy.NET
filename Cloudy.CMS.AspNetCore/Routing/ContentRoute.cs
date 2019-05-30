using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentControllerSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Routing
{
    public class ContentRoute : IRouter
    {
        IRouter DefaultRouter { get; }
        IContentRouter ContentRouter { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentControllerFinder ContentControllerFinder { get; }

        public ContentRoute(IRouter defaultRouter, IContentRouter contentRouter, IContentTypeProvider contentTypeRepository, IContentControllerFinder contentControllerFinder)
        {
            DefaultRouter = defaultRouter;
            ContentRouter = contentRouter;
            ContentTypeRepository = contentTypeRepository;
            ContentControllerFinder = contentControllerFinder;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            var segments = (IEnumerable<string>)context.HttpContext.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            IContent content;
            ContentTypeDescriptor contentType;

            if (segments.FirstOrDefault() == "Cloudy.CMS" && segments.Skip(1).FirstOrDefault() == "EditPage")
            {
                context.RouteData.Values["mode"] = "edit-on-page";
                segments = segments.Skip(2);
            }

            content = ContentRouter.RouteContent(segments, null);

            if (content == null)
            {
                return;
            }

            contentType = ContentTypeRepository.Get(content.GetType());

            var match = ContentControllerFinder.FindController(contentType);

            if(match == null)
            {
                return;
            }

            context.RouteData.Values["controller"] = match.ControllerName;
            context.RouteData.Values["action"] = match.ControllerAction;
            context.RouteData.Values["contentFromContentRoute"] = content;

            await DefaultRouter.RouteAsync(context);
        }
    }
}
