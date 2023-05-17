using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.EntitySupport.Reference;
using Cloudy.NET.Naming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cloudy.NET.EntitySupport;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace Cloudy.NET.UI.FieldSupport.Select
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class SelectPreviewController : Controller
    {
        IEntityTypeProvider EntityTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        INameGetter NameGetter { get; }
        IReferenceDeserializer ReferenceDeserializer { get; }

        public SelectPreviewController(IEntityTypeProvider entityTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine, INameGetter nameGetter, IReferenceDeserializer referenceDeserializer)
        {
            EntityTypeProvider = entityTypeProvider;
            ContextCreator = contextCreator;
            NameGetter = nameGetter;
            ReferenceDeserializer = referenceDeserializer;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/select/preview")]
        public async Task<IActionResult> GetCard(string entityType, string reference, bool simpleKey)
        {
            var type = EntityTypeProvider.Get(entityType);

            var deserializedReference = ReferenceDeserializer.Get(type.Type, reference, simpleKey);

            var context = ContextCreator.CreateFor(type.Type);

            var instance = await context.Context.FindAsync(type.Type, deserializedReference).ConfigureAwait(false);
            
            if(instance == null)
            {
                return NotFound();
            }

            return Json(new PreviewResult(
                NameGetter.GetName(instance),
                deserializedReference,
                (instance as IImageable)?.Image
            ), new JsonSerializerOptions().CloudyDefault());
        }

        public record PreviewResult(
            string Name,
            object Reference,
            string Image
        );
    }
}
