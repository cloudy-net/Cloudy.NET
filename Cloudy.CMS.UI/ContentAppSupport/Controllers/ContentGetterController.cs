using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class ContentGetterController : Controller
    {
        IContentGetter ContentGetter { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentJsonConverterProvider ContentJsonConverterProvider { get; }

        public ContentGetterController(IContentGetter contentGetter, IPrimaryKeyConverter primaryKeyConverter, IContentTypeProvider contentTypeProvider, IContentJsonConverterProvider contentJsonConverterProvider)
        {
            ContentGetter = contentGetter;
            PrimaryKeyConverter = primaryKeyConverter;
            ContentTypeProvider = contentTypeProvider;
            ContentJsonConverterProvider = contentJsonConverterProvider;
        }

        [HttpPost]
        public async Task<ActionResult> Get([FromBody] GetContentRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var options = new JsonSerializerOptions();
            ContentJsonConverterProvider.GetAll().ToList().ForEach(c => options.Converters.Add(c));
            var keys = PrimaryKeyConverter.Convert(data.KeyValues, data.ContentTypeId);
            var content = await ContentGetter.GetAsync(data.ContentTypeId, keys).ConfigureAwait(false);

            if(content == null)
            {
                return Json(new { NotFound = true });
            }

            return Json(content, options);
        }

        public class GetContentRequestBody
        {
            [Required]
            public JsonElement[] KeyValues { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
        }
    }
}
