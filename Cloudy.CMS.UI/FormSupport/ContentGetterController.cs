using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    [Authorize("adminarea")]
    [Area("Admin")]
    [ResponseCache(NoStore = true)]
    public class ContentGetterController : Controller
    {
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IContentJsonConverterProvider ContentJsonConverterProvider { get; }

        public ContentGetterController(IPrimaryKeyConverter primaryKeyConverter, IContentTypeProvider contentTypeProvider, IContextCreator contextCreator, IContentJsonConverterProvider contentJsonConverterProvider)
        {
            PrimaryKeyConverter = primaryKeyConverter;
            ContentTypeProvider = contentTypeProvider;
            ContextCreator = contextCreator;
            ContentJsonConverterProvider = contentJsonConverterProvider;
        }

        [HttpPost]
        [Route("/{area}/api/form/content/get")]
        public async Task<ActionResult> Get([FromBody] GetContentRequestBody payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var contentType = ContentTypeProvider.Get(payload.ContentType);
            var keys = PrimaryKeyConverter.Convert(payload.KeyValues, contentType.Type);
            var context = ContextCreator.CreateFor(contentType.Type);
            var content = await context.Context.FindAsync(contentType.Type, keys).ConfigureAwait(false);

            if(content == null)
            {
                return Json(new { NotFound = true });
            }

            var options = new JsonSerializerOptions();
            ContentJsonConverterProvider.GetAll().ToList().ForEach(options.Converters.Add);
            return Json(content, options);
        }

        public class GetContentRequestBody
        {
            [Required]
            public string[] KeyValues { get; set; }
            [Required]
            public string ContentType { get; set; }
        }
    }
}
