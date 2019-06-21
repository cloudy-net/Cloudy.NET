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

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [Area("Cloudy.CMS")]
    [Route("ContentApp")]
    public class ContentAppApiController
    {
        IContentTypeProvider ContentTypeRepository { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IUrlProvider UrlProvider { get; }
        ISingletonProvider SingletonProvider { get; }

        public ContentAppApiController(IContentTypeProvider contentTypeRepository, IContainerSpecificContentGetter containerSpecificContentGetter, IContainerSpecificContentCreator containerSpecificContentCreator, IContainerSpecificContentUpdater containerSpecificContentUpdater, IUrlProvider urlProvider, ISingletonProvider singletonProvider)
        {
            ContentTypeRepository = contentTypeRepository;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            UrlProvider = urlProvider;
            SingletonProvider = singletonProvider;
        }

        [Route("GetSingleton")]
        public IContent Get(string id)
        {
            var contentType = ContentTypeRepository.Get(id);

            var singleton = SingletonProvider.Get(id);

            return ContainerSpecificContentGetter.Get<IContent>(singleton.Id, null, contentType.Container);
        }

        [Route("Save")]
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

        [Route("GetUrl")]
        public string GetUrl(string id, string contentTypeId)
        {
            var contentType = ContentTypeRepository.Get(contentTypeId);

            var content = ContainerSpecificContentGetter.Get<IContent>(id, null, contentType.Container);

            return UrlProvider.Get(content);
        }
    }
}
