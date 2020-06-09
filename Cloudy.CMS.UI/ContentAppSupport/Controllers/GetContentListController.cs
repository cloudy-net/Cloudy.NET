using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public GetContentListController(IContentTypeProvider contentTypeRepository, IDocumentFinder documentFinder, IContentDeserializer contentDeserializer, IPropertyDefinitionProvider propertyDefinitionProvider)
        {
            ContentTypeProvider = contentTypeRepository;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
            PropertyDefinitionProvider = propertyDefinitionProvider;
        }

        [HttpGet]
        [Route("GetContentList")]
        public IEnumerable<object> GetContentList(string[] contentTypeId)
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

            return result.AsReadOnly();
        }
    }
}
