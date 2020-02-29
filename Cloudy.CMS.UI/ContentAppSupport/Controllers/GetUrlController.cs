using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("Content")]
    public class GetUrlController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IUrlProvider UrlProvider { get; }

        public GetUrlController(IContentTypeProvider contentTypeProvider, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        [HttpPost]
        [Route("GetUrl")]
        public string GetUrl(string id, string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            var content = ContainerSpecificContentGetter.Get<IContent>(id, null, contentType.Container);

            return UrlProvider.Get(content);
        }
    }
}
