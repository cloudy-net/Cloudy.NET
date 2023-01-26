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
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    [Authorize("adminarea")]
    [Area("Admin")]
    [ResponseCache(NoStore = true)]
    public class EntityGetterController : Controller
    {
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IEntityTypeProvider EntityTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IEmbeddedBlockJsonConverterProvider ContentJsonConverterProvider { get; }

        public EntityGetterController(IPrimaryKeyConverter primaryKeyConverter, IEntityTypeProvider entityTypeProvider, IContextCreator contextCreator, IEmbeddedBlockJsonConverterProvider contentJsonConverterProvider)
        {
            PrimaryKeyConverter = primaryKeyConverter;
            EntityTypeProvider = entityTypeProvider;
            ContextCreator = contextCreator;
            ContentJsonConverterProvider = contentJsonConverterProvider;
        }

        [HttpPost]
        [Route("/{area}/api/form/entity/get")]
        public async Task<ActionResult> Get([FromBody] RequestBody payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entityType = EntityTypeProvider.Get(payload.EntityType);
            var keys = PrimaryKeyConverter.Convert(payload.KeyValues, entityType.Type);
            var context = ContextCreator.CreateFor(entityType.Type);
            var entity = await context.Context.FindAsync(entityType.Type, keys).ConfigureAwait(false);

            if(entity == null)
            {
                return Json(new { NotFound = true });
            }

            var options = new JsonSerializerOptions();
            ContentJsonConverterProvider.GetAll().ToList().ForEach(options.Converters.Add);

            var value = JsonSerializer.SerializeToDocument(entity, options);

            return Json(new ResponseBody(value, new GetContentEntityType(new Dictionary<string, GetContentPropertyDefinition>()), new Dictionary<string, GetContentEntityType>()));
        }

        public class RequestBody
        {
            [Required]
            public string[] KeyValues { get; set; }
            [Required]
            public string EntityType { get; set; }
        }

        public record ResponseBody(JsonDocument Entity, GetContentEntityType Type, IDictionary<string, GetContentEntityType> SupportingTypes);

        public record GetContentEntityType(IDictionary<string, GetContentPropertyDefinition> Properties);

        public record GetContentPropertyDefinition(bool Simple, string Type);
    }
}
