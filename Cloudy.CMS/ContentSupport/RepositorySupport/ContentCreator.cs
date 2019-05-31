using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Bson.IO;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentCreator : IContentCreator
    {
        IContainerProvider ContainerProvider { get; }
        IIdGenerator IdGenerator { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentSerializer ContentSerializer { get; }

        public ContentCreator(IContainerProvider containerProvider, IIdGenerator idGenerator, IContentTypeProvider contentTypeRepository, IContentSerializer contentSerializer)
        {
            ContainerProvider = containerProvider;
            IdGenerator = idGenerator;
            ContentTypeRepository = contentTypeRepository;
            ContentSerializer = contentSerializer;
        }

        public void Create(IContent content)
        {
            CreateAsync(content).WaitAndUnwrapException();
        }

        public async Task CreateAsync(IContent content)
        {
            if (content.Id != null)
            {
                throw new InvalidOperationException($"This content seems to already exist as it has a non-null Id ({content.Id}). Did you mean to use IContentUpdater?");
            }

            var contentType = ContentTypeRepository.Get(content.GetType());

            if (contentType == null)
            {
                throw new InvalidOperationException($"This content has no content type (or rather its Type ({content.GetType()}) has no [ContentType] attribute)");
            }

            content.Id = IdGenerator.Generate();
            content.ContentTypeId = contentType.Id;

            await ContainerProvider.Get(ContainerConstants.Content).InsertOneAsync(ContentSerializer.Serialize(content, contentType));
        }
    }
}
