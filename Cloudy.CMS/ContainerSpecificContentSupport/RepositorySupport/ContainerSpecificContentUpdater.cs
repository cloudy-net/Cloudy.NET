using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Driver;
using Cloudy.CMS.ContentSupport;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public class ContainerSpecificContentUpdater : IContainerSpecificContentUpdater
    {
        IContainerProvider ContainerProvider { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentSerializer ContentSerializer { get; }

        public ContainerSpecificContentUpdater(IContainerProvider containerProvider, IContentTypeProvider contentTypeRepository, IContentSerializer contentSerializer)
        {
            ContainerProvider = containerProvider;
            ContentTypeRepository = contentTypeRepository;
            ContentSerializer = contentSerializer;
        }

        public void Update(IContent content, string container)
        {
            if (content.Id == null)
            {
                throw new InvalidOperationException($"This content cannot be updated as it doesn't seem to exist (Id is null). Did you mean to use IContentCreator?");
            }

            var contentType = ContentTypeRepository.Get(content.ContentTypeId);

            if (contentType == null)
            {
                throw new InvalidOperationException($"This content has no content type (or rather its base class has no [ContentType] attribute)");
            }

            var document = ContentSerializer.Serialize(content, contentType);

            ContainerProvider.Get(container).UpdateOne(Builders<Document>.Filter.Eq(d => d.Id, content.Id), Builders<Document>.Update.Set(d => d.GlobalFacet, document.GlobalFacet));
        }

        public async Task UpdateAsync(IContent content, string container)
        {
            if (content.Id == null)
            {
                throw new InvalidOperationException($"This content cannot be updated as it doesn't seem to exist (Id is null). Did you mean to use IContentCreator?");
            }

            var contentType = ContentTypeRepository.Get(content.ContentTypeId);

            if (contentType == null)
            {
                throw new InvalidOperationException($"This content has no content type (or rather its base class has no [ContentType] attribute)");
            }

            var document = ContentSerializer.Serialize(content, contentType);

            await ContainerProvider.Get(container).UpdateOneAsync(Builders<Document>.Filter.Eq(d => d.Id, content.Id), Builders<Document>.Update.Set(d => d.GlobalFacet, document.GlobalFacet));
        }
    }
}
