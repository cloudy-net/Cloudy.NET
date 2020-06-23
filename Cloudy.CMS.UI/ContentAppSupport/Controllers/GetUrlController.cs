using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize]
    [Area("Cloudy.CMS")]
    [Route("Content")]
    public class GetUrlController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IUrlProvider UrlProvider { get; }
        IContentRouteMatcher ContentRouteMatcher { get; }

        public GetUrlController(IContentTypeProvider contentTypeProvider, IContainerSpecificContentGetter containerSpecificContentGetter, IUrlProvider urlProvider, IContentRouteMatcher contentRouteMatcher)
        {
            ContentTypeProvider = contentTypeProvider;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            UrlProvider = urlProvider;
            ContentRouteMatcher = contentRouteMatcher;
        }

        [HttpGet]
        [Route("GetUrl")]
        public IEnumerable<string> GetUrl(string id, string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            var content = ContainerSpecificContentGetter.Get<IContent>(id, null, contentType.Container);

            var contentRouteSegment = UrlProvider.Get(content);

            var contentRoutes = ContentRouteMatcher.GetFor(contentType);

            var result = new List<string>();

            foreach(var contentRoute in contentRoutes)
            {
                result.Add(contentRoute.Apply(contentRouteSegment));
            }

            return result.AsReadOnly();
        }
    }
}
