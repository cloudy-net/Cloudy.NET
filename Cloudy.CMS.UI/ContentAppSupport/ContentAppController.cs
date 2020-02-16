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
using Newtonsoft.Json.Serialization;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [Area("Cloudy.CMS")]
    public class ContentAppController
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IUrlProvider UrlProvider { get; }
        ISingletonProvider SingletonProvider { get; }
        IPluralizer Pluralizer { get; }
        IHumanizer Humanizer { get; }
        IDocumentFinder DocumentFinder { get; }
        IContentDeserializer ContentDeserializer { get; }
        INameExpressionParser NameExpressionParser { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();
        IListActionModuleProvider ListActionModuleProvider { get; }
        IContentTypeActionModuleProvider ContentTypeActionModuleProvider { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }

        public ContentAppController(IContentTypeProvider contentTypeRepository, IContainerSpecificContentGetter containerSpecificContentGetter, IContainerSpecificContentCreator containerSpecificContentCreator, IContainerSpecificContentUpdater containerSpecificContentUpdater, IUrlProvider urlProvider, ISingletonProvider singletonProvider, IPluralizer pluralizer, IHumanizer humanizer, IDocumentFinder documentFinder, IContentDeserializer contentDeserializer, INameExpressionParser nameExpressionParser, IListActionModuleProvider listActionModuleProvider, IContentTypeActionModuleProvider contentTypeActionModuleProvider, IPropertyDefinitionProvider propertyDefinitionProvider)
        {
            ContentTypeProvider = contentTypeRepository;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            UrlProvider = urlProvider;
            SingletonProvider = singletonProvider;
            Pluralizer = pluralizer;
            Humanizer = humanizer;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
            NameExpressionParser = nameExpressionParser;
            ListActionModuleProvider = listActionModuleProvider;
            ContentTypeActionModuleProvider = contentTypeActionModuleProvider;
            PropertyDefinitionProvider = propertyDefinitionProvider;
        }

        public IEnumerable<ContentTypeResponseItem> GetContentTypes()
        {
            var result = new List<ContentTypeResponseItem>();
            
            foreach(var contentType in ContentTypeProvider.GetAll())
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
                    NameablePropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? CamelCaseNamingStrategy.GetPropertyName(NameExpressionParser.Parse(contentType.Type), false) : null,
                    IsRoutable = typeof(IRoutable).IsAssignableFrom(contentType.Type),
                    IsSingleton = singleton != null,
                    Count = -1,
                    ContentTypeActionModules = ContentTypeActionModuleProvider.GetContentTypeActionModulesFor(contentType.Id),
                    ListActionModules = ListActionModuleProvider.GetListActionModulesFor(contentType.Id),
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
            public string NameablePropertyName { get; set; }
            public bool IsRoutable { get; set; }
            public bool IsSingleton { get; set; }
            public string SingletonId { get; set; }
            public int Count { get; set; }
            public IEnumerable<string> ContentTypeActionModules { get; set; }
            public IEnumerable<string> ListActionModules { get; set; }
        }

        public IEnumerable<object> GetContentList(string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            var documents = DocumentFinder.Find(contentType.Container).WhereEquals<IContent, string>(x => x.ContentTypeId, contentType.Id).GetResultAsync().Result.ToList();

            var result = new List<object>();

            foreach (var document in documents)
            {
                result.Add(ContentDeserializer.Deserialize(document, contentType, DocumentLanguageConstants.Global));
            }

            var sortByPropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? "Name" : "Id";
            var sortByProperty = PropertyDefinitionProvider.GetFor(contentType.Id).FirstOrDefault(p => p.Name == sortByPropertyName);

            if (sortByProperty != null)
            {
                result = result.OrderBy(i => sortByProperty.Getter(i)).ToList();
            }

            return result.AsReadOnly();
        }

        public IContent GetSingleton(string id)
        {
            var contentType = ContentTypeProvider.Get(id);

            var singleton = SingletonProvider.Get(id);

            return ContainerSpecificContentGetter.Get<IContent>(singleton.Id, null, contentType.Container);
        }

        public string SaveContent([FromBody] SaveContentRequestBody data)
        {
            var contentType = ContentTypeProvider.Get(data.ContentTypeId);

            var b = (IContent)JsonConvert.DeserializeObject(data.Content, contentType.Type);

            if (b.Id != null)
            {
                var a = (IContent)typeof(IContainerSpecificContentGetter).GetMethod(nameof(ContainerSpecificContentGetter.Get)).MakeGenericMethod(contentType.Type).Invoke(ContainerSpecificContentGetter, new[] { data.Id, null, contentType.Container });

                foreach(var propertyDefinition in PropertyDefinitionProvider.GetFor(contentType.Id))
                {
                    var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

                    if(display != null && display.GetAutoGenerateField() == false)
                    {
                        continue;
                    }

                    propertyDefinition.Setter(a, propertyDefinition.Getter(b));
                }

                ContainerSpecificContentUpdater.Update(a, contentType.Container);

                return "Updated";
            }
            else
            {
                ContainerSpecificContentCreator.Create(b, contentType.Container);

                return "Saved";
            }
        }

        public class SaveContentRequestBody
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
            public string Content { get; set; }
        }
        
        public string GetUrl(string id, string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            var content = ContainerSpecificContentGetter.Get<IContent>(id, null, contentType.Container);

            return UrlProvider.Get(content);
        }
    }
}
