using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("Content")]
    public class RemoveContentController : Controller
    {
        IContainerSpecificContentDeleter ContainerSpecificContentDeleter { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }

        public RemoveContentController(IContainerSpecificContentDeleter containerSpecificContentDeleter, IContentTypeProvider contentTypeProvider, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContainerSpecificContentDeleter = containerSpecificContentDeleter;
            ContentTypeProvider = contentTypeProvider;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        [HttpPost]
        [Route("RemoveContent")]
        public object RemoveContent([FromBody] RemoveContentInput removeContentInput)
        {
            if (!ModelState.IsValid)
            {
                return new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { description = e.ErrorMessage }),
                };
            }

            var contentType = ContentTypeProvider.Get(removeContentInput.ContentTypeId);
            var content = ContainerSpecificContentGetter.Get<IContent>(removeContentInput.Id, null, contentType.Container);

            if(content.ContentTypeId != contentType.Id)
            {
                throw new Exception($"Content is not of type {contentType.Id}");
            }

            ContainerSpecificContentDeleter.Delete(content.Id, contentType.Container);

            return new
            {
                success = true,
            };
        }

        public class RemoveContentInput
        {
            [Required]
            public string Id { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
        }
    }
}
