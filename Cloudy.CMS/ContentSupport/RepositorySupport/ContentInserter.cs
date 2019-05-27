using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentInserter : IContentInserter
    {
        IDocumentRepository DocumentRepository { get; }
        IIdGenerator IdGenerator { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentSerializer ContentSerializer { get; }

        public ContentInserter(IDocumentRepository documentRepository, IIdGenerator idGenerator, IContentTypeProvider contentTypeRepository, IContentSerializer contentSerializer)
        {
            DocumentRepository = documentRepository;
            IdGenerator = idGenerator;
            ContentTypeRepository = contentTypeRepository;
            ContentSerializer = contentSerializer;
        }

        public void Insert(IContent content)
        {
            InsertAsync(content).WaitAndUnwrapException();
        }

        public async Task InsertAsync(IContent content)
        {
            if (content.Id == null)
            {
                throw new InvalidOperationException($"This content has no Id. Inserting (sorry for the confusing terminology) is for forcing newly created content to have a set Id. Did you mean to use IContentCreator?");
            }

            var contentType = ContentTypeRepository.Get(content.GetType());

            if (contentType == null)
            {
                throw new InvalidOperationException($"This content has no content type (or rather its Type ({content.GetType()}) has no [ContentType] attribute)");
            }

            content.ContentTypeId = contentType.Id;

            await DocumentRepository.Documents.InsertOneAsync(ContentSerializer.Serialize(content, contentType));
        }
    }
}
