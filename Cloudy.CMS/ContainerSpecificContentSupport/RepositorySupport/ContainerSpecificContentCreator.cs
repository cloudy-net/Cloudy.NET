using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public class ContainerSpecificContentCreator : IContainerSpecificContentCreator
    {
        IDocumentCreator DocumentCreator { get; }
        IIdGenerator IdGenerator { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentSerializer ContentSerializer { get; }

        public ContainerSpecificContentCreator(IDocumentCreator documentCreator, IIdGenerator idGenerator, IContentTypeProvider contentTypeRepository, IContentSerializer contentSerializer)
        {
            DocumentCreator = documentCreator;
            IdGenerator = idGenerator;
            ContentTypeRepository = contentTypeRepository;
            ContentSerializer = contentSerializer;
        }

        public void Create(IContent content, string container)
        {
            CreateAsync(content, container).WaitAndUnwrapException();
        }

        public async Task CreateAsync(IContent content, string container)
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

            await DocumentCreator.Create(container, ContentSerializer.Serialize(content, contentType));
        }
    }
}
