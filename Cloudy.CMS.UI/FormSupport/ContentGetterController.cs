using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.EntitySupport.Serialization;
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
        IEntityTypeProvider EntityTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IEmbeddedBlockJsonConverterProvider ContentJsonConverterProvider { get; }

        public ContentGetterController(IPrimaryKeyConverter primaryKeyConverter, IEntityTypeProvider entityTypeProvider, IContextCreator contextCreator, IEmbeddedBlockJsonConverterProvider contentJsonConverterProvider)
        {
            PrimaryKeyConverter = primaryKeyConverter;
            EntityTypeProvider = entityTypeProvider;
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

            var entityType = EntityTypeProvider.Get(payload.EntityType);
            var keys = PrimaryKeyConverter.Convert(payload.KeyValues, entityType.Type);
            var context = ContextCreator.CreateFor(entityType.Type);
            var content = await context.Context.FindAsync(entityType.Type, keys).ConfigureAwait(false);

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
            public string EntityType { get; set; }
        }
    }
}
