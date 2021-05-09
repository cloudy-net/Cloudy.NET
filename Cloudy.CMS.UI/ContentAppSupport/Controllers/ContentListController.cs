using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
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
        IContentFinder ContentFinder { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IContentChildrenCounter ContentChildrenCounter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        PolymorphicFormConverter PolymorphicFormConverter { get; }

        public ContentListController(IContentTypeProvider contentTypeRepository, IContentFinder contentFinder, IPrimaryKeyGetter primaryKeyGetter, IContentChildrenCounter contentChildrenCounter, IPropertyDefinitionProvider propertyDefinitionProvider, PolymorphicFormConverter polymorphicFormConverter)
        {
            ContentTypeProvider = contentTypeRepository;
            ContentFinder = contentFinder;
            PrimaryKeyGetter = primaryKeyGetter;
            ContentChildrenCounter = contentChildrenCounter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            PolymorphicFormConverter = polymorphicFormConverter;
        }

        public async Task Get(string[] contentTypeIds, string parent)
        {
            var contentTypes = contentTypeIds.Select(t => ContentTypeProvider.Get(t)).ToList().AsReadOnly();

            if(contentTypes.Count > 1)
            {
                throw new NotImplementedException("Multi type queries are not yet implemented");
            }

            var items = new List<object>();
            var itemChildrenCounts = new Dictionary<string, int>();

            var documentsQuery = ContentFinder.Find(contentTypes.Single().Type);

            if(parent != null)
            {
                documentsQuery.WhereParent(parent);
            }
            else
            {
                documentsQuery.WhereHasNoParent();
            }

            var documents = (await documentsQuery.GetResultAsync().ConfigureAwait(false)).ToList();

            foreach (var content in documents)
            {
                items.Add(content);

                if (content is IHierarchical)
                {
                    var id = PrimaryKeyGetter.Get(content);

                    itemChildrenCounts[string.Join(",", id)] = await ContentChildrenCounter.CountChildrenForAsync(id).ConfigureAwait(false);
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
            public IEnumerable<object> Items { get; set; }
        }
    }
}
