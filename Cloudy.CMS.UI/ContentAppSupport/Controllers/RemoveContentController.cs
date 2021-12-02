using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class RemoveContentController : Controller
    {
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContentDeleter ContentDeleter { get; }

        public RemoveContentController(IPrimaryKeyConverter primaryKeyConverter, IContentDeleter contentDeleter)
        {
            PrimaryKeyConverter = primaryKeyConverter;
            ContentDeleter = contentDeleter;
        }

        [HttpPost]
        public async Task<object> RemoveContent([FromBody] RemoveContentInput removeContentInput)
        {
            if (!ModelState.IsValid)
            {
                return new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { description = e.ErrorMessage }),
                };
            }

            await ContentDeleter.DeleteAsync(removeContentInput.ContentTypeId, PrimaryKeyConverter.Convert(removeContentInput.KeyValues, removeContentInput.ContentTypeId)).ConfigureAwait(false);

            return new
            {
                success = true,
            };
        }

        public class RemoveContentInput
        {
            [Required]
            public JsonElement[] KeyValues { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
        }
    }
}
