using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class ContentGetterController
    {
        IContentTypeProvider ContentTypeProvider { get; }

        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }

        public ContentGetterController(IContentTypeProvider contentTypeRepository, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContentTypeProvider = contentTypeRepository;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        public async Task<IContent> GetAsync(string id, string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            return await ContainerSpecificContentGetter.GetAsync(id, null, contentType.Container);
        }
    }
}
