using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.SingletonSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("Content")]
    public class GetSingletonController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        ISingletonProvider SingletonProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }

        public GetSingletonController(IContentTypeProvider contentTypeProvider, ISingletonProvider singletonProvider, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            SingletonProvider = singletonProvider;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        [HttpGet]
        [Route("GetSingleton")]
        public IContent GetSingleton(string id)
        {
            var contentType = ContentTypeProvider.Get(id);

            var singleton = SingletonProvider.Get(id);

            return ContainerSpecificContentGetter.Get<IContent>(singleton.Id, null, contentType.Container);
        }
    }
}
