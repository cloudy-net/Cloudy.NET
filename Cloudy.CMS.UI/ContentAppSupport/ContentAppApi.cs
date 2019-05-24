using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Poetry.UI.ApiSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [Api("ContentApp")]
    public class ContentAppApi
    {
        IContentTypeProvider ContentTypeRepository { get; }
        IContentCreator ContentCreator { get; }
        IContentUpdater ContentUpdater { get; }

        public ContentAppApi(IContentTypeProvider contentTypeRepository, IContentCreator contentCreator, IContentUpdater contentUpdater)
        {
            ContentTypeRepository = contentTypeRepository;
            ContentCreator = contentCreator;
            ContentUpdater = contentUpdater;
        }

        [Endpoint("Save")]
        public string Save([FromRequestBody] SaveData data)
        {
            var contentType = ContentTypeRepository.Get(data.ContentTypeId);

            var item = (IContent)data.Item.ToObject(contentType.Type);

            item.ContentTypeId = contentType.Id;

            if (item.Id != null)
            {
                ContentUpdater.Update(item);

                return "Updated";
            }
            else
            {
                ContentCreator.Create(item);

                return "Saved";
            }
        }
        
        public class SaveData
        {
            public string Id { get; set; }
            public string ContentTypeId { get; set; }
            public JObject Item { get; set; }
        }
    }
}
