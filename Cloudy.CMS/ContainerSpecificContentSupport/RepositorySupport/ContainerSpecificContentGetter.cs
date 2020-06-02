using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public class ContainerSpecificContentGetter : IContainerSpecificContentGetter
    {
        IDocumentGetter DocumentGetter { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentDeserializer ContentDeserializer { get; }

        public ContainerSpecificContentGetter(IDocumentGetter documentGetter, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            DocumentGetter = documentGetter;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }

        public T Get<T>(string id, string language, string container) where T : class
        {
            return GetAsync<T>(id, language, container).WaitAndUnwrapException();
        }

        public async Task<IContent> GetAsync(string id, string language, string container)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (language == null)
            {
                language = DocumentLanguageConstants.Global;
            }

            var document = await DocumentGetter.GetAsync(container, id);

            if (document == null)
            {
                return null;
            }

            var contentType = ContentTypeRepository.Get(document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string);

            return ContentDeserializer.Deserialize(document, contentType, language);
        }

        public async Task<T> GetAsync<T>(string id, string language, string container) where T : class
        {
            return (T)await GetAsync(id, language, container);
        }
    }
}
