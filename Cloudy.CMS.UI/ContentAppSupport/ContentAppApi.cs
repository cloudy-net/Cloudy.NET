using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Poetry.UI.ApiSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [Api("ContentApp")]
    public class ContentAppApi
    {
        IContentTypeProvider ContentTypeRepository { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IUrlProvider UrlProvider { get; }
        ISingletonProvider SingletonProvider { get; }

        public ContentAppApi(IContentTypeProvider contentTypeRepository, IContainerSpecificContentGetter containerSpecificContentGetter, IContainerSpecificContentCreator containerSpecificContentCreator, IContainerSpecificContentUpdater containerSpecificContentUpdater, IUrlProvider urlProvider, ISingletonProvider singletonProvider)
        {
            ContentTypeRepository = contentTypeRepository;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            UrlProvider = urlProvider;
            SingletonProvider = singletonProvider;
        }

        [Endpoint("GetSingleton")]
        public IContent Get(string id)
        {
            var contentType = ContentTypeRepository.Get(id);

            var singleton = SingletonProvider.Get(id);

            return ContainerSpecificContentGetter.Get<IContent>(singleton.Id, null, contentType.Container);
        }

        [Endpoint("Save")]
        public string Save([FromRequestBody] SaveData data)
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

        [Endpoint("GetUrl")]
        public string GetUrl(string id, string contentTypeId)
        {
            var contentType = ContentTypeRepository.Get(contentTypeId);

            var content = ContainerSpecificContentGetter.Get<IContent>(id, null, contentType.Container);

            return UrlProvider.Generate(content);
        }
    }
}
