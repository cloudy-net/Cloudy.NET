using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("Content")]
    public class GetContentListController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDocumentFinder DocumentFinder { get; }
        IContentDeserializer ContentDeserializer { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        PolymorphicFormConverter PolymorphicFormConverter { get; }

        public GetContentListController(IContentTypeProvider contentTypeRepository, IDocumentFinder documentFinder, IContentDeserializer contentDeserializer, IPropertyDefinitionProvider propertyDefinitionProvider, PolymorphicFormConverter polymorphicFormConverter)
        {
            ContentTypeProvider = contentTypeRepository;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            PolymorphicFormConverter = polymorphicFormConverter;
        }

        [HttpGet]
        [Route("GetContentList")]
        public async Task GetContentList(string[] contentTypeId)
        {
            var result = new List<object>();

            foreach (var contentType in contentTypeId.Select(t => ContentTypeProvider.Get(t)))
            {
                var documents = DocumentFinder.Find(contentType.Container).WhereEquals<IContent, string>(x => x.ContentTypeId, contentType.Id).GetResultAsync().Result.ToList();

                foreach (var document in documents)
                {
                    result.Add(ContentDeserializer.Deserialize(document, contentType, DocumentLanguageConstants.Global));
                }
            }

            //var sortByPropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? "Name" : "Id";
            //var sortByProperty = PropertyDefinitionProvider.GetFor(contentType.Id).FirstOrDefault(p => p.Name == sortByPropertyName);

            //if (sortByProperty != null)
            //{
            //    result = result.OrderBy(i => sortByProperty.Getter(i)).ToList();
            //}

            Response.ContentType = "application/json";

            await Response.WriteAsync(JsonConvert.SerializeObject(result.AsReadOnly(), new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver(), Converters = new List<JsonConverter> { PolymorphicFormConverter } }));
        }
    }
}
