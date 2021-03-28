using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentGetter : IContentGetter
    {
        IDocumentGetter DocumentGetter { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContentGetter(IDocumentGetter documentGetter, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            DocumentGetter = documentGetter;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }

        public async Task<T> GetAsync<T>(string id, string language) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (language == null)
            {
                language = DocumentLanguageConstants.Global;
            }

            var contentType = ContentTypeRepository.Get(typeof(T));

            if (contentType == null)
            {
                throw new TypeNotRegisteredContentTypeException(typeof(T));
            }

            var document = await DocumentGetter.GetAsync(contentType.Container, id);

            if (document == null)
            {
                return null;
            }

            return (T)ContentDeserializer.Deserialize(document, contentType, language);
        }

        public async Task<IContent> GetAsync(string contentTypeId, string id, string language)
        {
            if (contentTypeId == null)
            {
                throw new ArgumentNullException(nameof(contentTypeId));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (language == null)
            {
                language = DocumentLanguageConstants.Global;
            }

            var contentType = ContentTypeRepository.Get(contentTypeId);

            if (contentType == null)
            {
                throw new ArgumentException(nameof(contentTypeId));
            }

            var document = await DocumentGetter.GetAsync(contentType.Container, id);

            if (document == null)
            {
                return null;
            }

            return ContentDeserializer.Deserialize(document, contentType, language);
        }
    }
}
