using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.EntitySupport.Reference;
using Cloudy.CMS.Naming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport.FieldTypes
{
    public class SelectPreviewController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        INameGetter NameGetter { get; }
        IReferenceDeserializer ReferenceDeserializer { get; }

        public SelectPreviewController(IContentTypeProvider contentTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine, INameGetter nameGetter, IReferenceDeserializer referenceDeserializer)
        {
            ContentTypeProvider = contentTypeProvider;
            ContextCreator = contextCreator;
            NameGetter = nameGetter;
            ReferenceDeserializer = referenceDeserializer;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/select/preview")]
        public async Task<IActionResult> GetCard(string contentType, string reference, bool simpleKey)
        {
            var type = ContentTypeProvider.Get(contentType);

            var deserializedReference = ReferenceDeserializer.Get(type.Type, reference, simpleKey);

            using var context = ContextCreator.CreateFor(type.Type);

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
