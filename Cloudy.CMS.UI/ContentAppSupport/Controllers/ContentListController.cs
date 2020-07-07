using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class ContentListController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDocumentFinder DocumentFinder { get; }
        IContentDeserializer ContentDeserializer { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        PolymorphicFormConverter PolymorphicFormConverter { get; }

        public ContentListController(IContentTypeProvider contentTypeRepository, IDocumentFinder documentFinder, IContentDeserializer contentDeserializer, IPropertyDefinitionProvider propertyDefinitionProvider, PolymorphicFormConverter polymorphicFormConverter)
        {
            ContentTypeProvider = contentTypeRepository;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            PolymorphicFormConverter = polymorphicFormConverter;
        }

        public async Task Get(string[] contentTypeIds, string parent)
        {
            var contentTypes = contentTypeIds.Select(t => ContentTypeProvider.Get(t)).ToList().AsReadOnly();
            var containers = contentTypes.Select(t => t.Container).Distinct().ToList();

            if(containers.Count > 1)
            {
                throw new ContentTypesSpanSeveralContainersException(contentTypes);
            }

            var hierarchical = contentTypes.Any(t => typeof(IHierarchical).IsAssignableFrom(t.Type));

            var items = new List<IContent>();
            var itemChildrenCounts = new Dictionary<string, int>();

            var documentsQuery = DocumentFinder.Find(containers.Single()).WhereIn<IContent, string>(x => x.ContentTypeId, contentTypeIds);

            if (hierarchical)
            {
                if(parent != null)
                {
                    documentsQuery.WhereEquals<IHierarchical, string>(x => x.ParentId, parent);
                }
                else
                {
                    documentsQuery.WhereNullOrMissing<IHierarchical>(x => x.ParentId);
                }
            }

            var documents = (await documentsQuery.GetResultAsync().ConfigureAwait(false)).ToList();

            foreach (var document in documents)
            {
                var contentTypeId = (string)document.GlobalFacet.Interfaces[nameof(IContent)].Properties[nameof(IContent.ContentTypeId)];
                var content = ContentDeserializer.Deserialize(document, ContentTypeProvider.Get(contentTypeId), DocumentLanguageConstants.Global);
                
                items.Add(content);

                if (hierarchical)
                {
                    itemChildrenCounts[content.Id] = (await DocumentFinder.Find(containers.Single()).WhereEquals<IHierarchical, string>(x => x.ParentId, content.Id).GetResultAsync()).Count();
                }
            }

            //var sortByPropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? "Name" : "Id";
            //var sortByProperty = PropertyDefinitionProvider.GetFor(contentType.Id).FirstOrDefault(p => p.Name == sortByPropertyName);

            //if (sortByProperty != null)
            //{
            //    result = result.OrderBy(i => sortByProperty.Getter(i)).ToList();
            //}

            Response.ContentType = "application/json";

            await Response.WriteAsync(JsonConvert.SerializeObject(new ContentListResult { Items = items, ItemChildrenCounts = itemChildrenCounts }, new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver(), Converters = new List<JsonConverter> { PolymorphicFormConverter } }));
        }

        public class ContentListResult
        {
            public IDictionary<string, int> ItemChildrenCounts { get; set; }
            public IEnumerable<IContent> Items { get; set; }
        }
    }
}
