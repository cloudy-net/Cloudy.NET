using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("Data/ContentGetter")]
    public class ContentGetterController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }

        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }

        public ContentGetterController(IContentTypeProvider contentTypeRepository, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContentTypeProvider = contentTypeRepository;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IContent> GetAsync(string id, string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            return await ContainerSpecificContentGetter.GetAsync(id, null, contentType.Container);
        }
    }
}
