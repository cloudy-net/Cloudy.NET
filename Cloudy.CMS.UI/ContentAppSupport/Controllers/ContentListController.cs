using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.SingletonSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
        ISingletonProvider SingletonProvider { get; }
        IHumanizer Humanizer { get; }

        public ContentListController(IContentTypeProvider contentTypeRepository, IContentFinder contentFinder, IPrimaryKeyGetter primaryKeyGetter, IContentChildrenCounter contentChildrenCounter, IPropertyDefinitionProvider propertyDefinitionProvider, ISingletonProvider singletonProvider, IHumanizer humanizer)
        {
            ContentTypeProvider = contentTypeRepository;
            ContentFinder = contentFinder;
            PrimaryKeyGetter = primaryKeyGetter;
            ContentChildrenCounter = contentChildrenCounter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            SingletonProvider = singletonProvider;
            Humanizer = humanizer;
        }

        public async Task<ActionResult> Get(string[] contentTypeIds, string parent)
        {
            var contentTypes = contentTypeIds.Select(t => ContentTypeProvider.Get(t)).ToList().AsReadOnly();

            if(contentTypes.Count > 1)
            {
                throw new NotImplementedException("Multi type queries are not yet implemented");
            }

            var items = new List<ContentListResultItem>();
            var itemChildrenCounts = new Dictionary<string, int>();

            var query = ContentFinder.Find(contentTypes.Single().Type);

            if(parent != null)
            {
                query.WhereParent(parent);
            }
            else
            {
                query.WhereHasNoParent();
            }

            var result = (await query.GetResultAsync().ConfigureAwait(false)).ToList();

            foreach (var content in result)
            {
                var contentType = ContentTypeProvider.Get(content.GetType());
                var item = new ContentListResultItem
                {
                    ContentTypeId = contentType.Id,
                    Keys = PrimaryKeyGetter.Get(content).ToList().AsReadOnly(),
                };

                var singleton = SingletonProvider.Get(contentType.Id);

                if (singleton == null)
                {
                    if (content is INameable)
                    {
                        item.Name = ((INameable)content).Name;
                    }
                }

                if (item.Name == null)
                {
                    var contentTypeName = contentType.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? contentType.Type.Name;
                    
                    if (contentTypeName.Contains(':') && !contentType.Id.Contains(':'))
                    {
                        contentTypeName = contentTypeName.Split(':').First();
                    }
                    else
                    {
                        contentTypeName = Humanizer.Humanize(contentTypeName);
                    }

                    if (singleton == null)
                    {
                        item.Name = $"{contentTypeName} {JsonSerializer.Serialize(item.Keys)}";
                    }
                    else
                    {
                        item.Name = contentTypeName;
                    }
                }

                items.Add(item);

                if (content is IHierarchical)
                {
                    itemChildrenCounts[JsonSerializer.Serialize(item.Keys)] = await ContentChildrenCounter.CountChildrenForAsync(item.Keys).ConfigureAwait(false);
                }
            }

            //var sortByPropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? "Name" : "Id";
            //var sortByProperty = PropertyDefinitionProvider.GetFor(contentType.Id).FirstOrDefault(p => p.Name == sortByPropertyName);

            //if (sortByProperty != null)
            //{
            //    result = result.OrderBy(i => sortByProperty.Getter(i)).ToList();
            //}

            var options = new JsonSerializerOptions
            {
                Converters = { new ContentJsonConverter(ContentTypeProvider) },
            };

            return Content(JsonSerializer.Serialize(new ContentListResult
            {
                Items = items,
                ItemChildrenCounts = itemChildrenCounts
            }, options));
        }

        public class ContentListResult
        {
            public IDictionary<string, int> ItemChildrenCounts { get; set; }
            public IEnumerable<ContentListResultItem> Items { get; set; }

        }
        public class ContentListResultItem
        {
            public string ContentTypeId { get; set; }
            public IEnumerable<object> Keys { get; set; }
            public string Name { get; set; }
        }
    }
}
