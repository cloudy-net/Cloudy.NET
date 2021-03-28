using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentUpdater : IContentUpdater
    {
        IDocumentUpdater DocumentUpdater { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        ISaveListenerProvider SaveListenerProvider { get; }
        IContentSerializer ContentSerializer { get; }

        public ContentUpdater(IDocumentUpdater documentUpdater, IContentTypeProvider contentTypeRepository, ISaveListenerProvider saveListenerProvider, IContentSerializer contentSerializer)
        {
            DocumentUpdater = documentUpdater;
            ContentTypeRepository = contentTypeRepository;
            SaveListenerProvider = saveListenerProvider;
            ContentSerializer = contentSerializer;
        }

        public async Task UpdateAsync(IContent content)
        {
            if (content.Id == null)
            {
                throw new InvalidOperationException($"This content cannot be updated as it doesn't seem to exist (Id is null). Did you mean to use IContentCreator?");
            }

            var contentType = ContentTypeRepository.Get(content.ContentTypeId);

            if (contentType == null)
            {
                throw new TypeNotRegisteredContentTypeException(content.GetType());
            }

            foreach (var saveListener in SaveListenerProvider.GetFor(content))
            {
                saveListener.BeforeSave(content);
            }

            var document = ContentSerializer.Serialize(content, contentType);

            await DocumentUpdater.UpdateAsync(contentType.Container, content.Id, document);
        }
    }
}
