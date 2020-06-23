using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.SingletonSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize]
    [Area("Cloudy.CMS")]
    public class SingletonController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        ISingletonProvider SingletonProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }

        public SingletonController(IContentTypeProvider contentTypeProvider, ISingletonProvider singletonProvider, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            SingletonProvider = singletonProvider;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        [HttpGet]
        public IContent Get(string id)
        {
            var contentType = ContentTypeProvider.Get(id);

            var singleton = SingletonProvider.Get(id);

            return ContainerSpecificContentGetter.Get<IContent>(singleton.Id, null, contentType.Container);
        }
    }
}
