using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport.Serialization;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [Area("Cloudy.CMS")]
    public class ContentAppController
    {
        IContentTypeProvider ContentTypeRepository { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IUrlProvider UrlProvider { get; }
        ISingletonProvider SingletonProvider { get; }
        IPluralizer Pluralizer { get; }
        IHumanizer Humanizer { get; }
        IDocumentFinder DocumentFinder { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContentAppController(IContentTypeProvider contentTypeRepository, IContainerSpecificContentGetter containerSpecificContentGetter, IContainerSpecificContentCreator containerSpecificContentCreator, IContainerSpecificContentUpdater containerSpecificContentUpdater, IUrlProvider urlProvider, ISingletonProvider singletonProvider, IPluralizer pluralizer, IHumanizer humanizer, IDocumentFinder documentFinder, IContentDeserializer contentDeserializer)
        {
            ContentTypeRepository = contentTypeRepository;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            UrlProvider = urlProvider;
            SingletonProvider = singletonProvider;
            Pluralizer = pluralizer;
            Humanizer = humanizer;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
        }

        public IEnumerable<ContentTypeResponseItem> GetContentTypes()
        {
            var result = new List<ContentTypeResponseItem>();
            
            foreach(var contentType in ContentTypeRepository.GetAll())
            {
                var name = contentType.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? contentType.Type.Name;
                string pluralName;

                if (name.Contains(':') && !contentType.Id.Contains(':'))
                {
                    var nameSplit = name.Split(':');

                    name = nameSplit.First();
                    pluralName = nameSplit.Last();
                }
                else
                {
                    name = Humanizer.Humanize(name);
                    pluralName = Pluralizer.Pluralize(name);
                }

                var singleton = SingletonProvider.Get(contentType.Id);

                result.Add(new ContentTypeResponseItem
                {
                    Id = contentType.Id,
                    Name = name,
                    PluralName = pluralName,
                    IsNameable = typeof(INameable).IsAssignableFrom(contentType.Type),
                    IsRoutable = typeof(IRoutable).IsAssignableFrom(contentType.Type),
                    IsSingleton = singleton != null,
                    Count = -1,
                });
            }

            return result.AsReadOnly();
        }

        public class ContentTypeResponseItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PluralName { get; set; }
            public bool IsNameable { get; set; }
            public bool IsRoutable { get; set; }
            public bool IsSingleton { get; set; }
            public string SingletonId { get; set; }
            public int Count { get; set; }
        }

        public IEnumerable<object> GetContentList(string contentTypeId)
        {
            var contentType = ContentTypeRepository.Get(contentTypeId);

            var documents = DocumentFinder.Find(contentType.Container).WhereEquals<IContent, string>(x => x.ContentTypeId, contentType.Id).GetResultAsync().Result.ToList();

            var result = new List<object>();

            foreach (var document in documents)
            {
                result.Add(ContentDeserializer.Deserialize(document, contentType, DocumentLanguageConstants.Global));
            }

            var sortByPropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? "Name" : "Id";
            var sortByProperty = contentType.PropertyDefinitions.FirstOrDefault(p => p.Name == sortByPropertyName);

            if (sortByProperty != null)
            {
                result = result.OrderBy(i => sortByProperty.Getter(i)).ToList();
            }

            return result.AsReadOnly();
        }

        public IContent GetSingleton(string id)
        {
            var contentType = ContentTypeRepository.Get(id);

            var singleton = SingletonProvider.Get(id);

            return ContainerSpecificContentGetter.Get<IContent>(singleton.Id, null, contentType.Container);
        }

        public string Save([FromBody] SaveData data)
        {
            var contentType = ContentTypeRepository.Get(data.ContentTypeId);

            var item = (IContent)data.Item.ToObject(contentType.Type);

            item.ContentTypeId = contentType.Id;

            if (item.Id != null)
            {
                ContainerSpecificContentUpdater.Update(item, contentType.Container);

                return "Updated";
            }
            else
            {
                ContainerSpecificContentCreator.Create(item, contentType.Container);

                return "Saved";
            }
        }
        
        public class SaveData
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
            public JObject Item { get; set; }
        }

        public string GetUrl(string id, string contentTypeId)
        {
            var contentType = ContentTypeRepository.Get(contentTypeId);

            var content = ContainerSpecificContentGetter.Get<IContent>(id, null, contentType.Container);

            return UrlProvider.Get(content);
        }
    }
}
