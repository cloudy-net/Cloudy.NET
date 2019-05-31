using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Driver;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentGetter : IContentGetter
    {
        IContainerProvider ContainerProvider { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        string Container { get; } = "content";
        IContentDeserializer ContentDeserializer { get; }

        public ContentGetter(IContainerProvider containerProvider, IContentTypeProvider contentTypeRepository, IContentDeserializer contentDeserializer)
        {
            ContainerProvider = containerProvider;
            ContentTypeRepository = contentTypeRepository;
            ContentDeserializer = contentDeserializer;
        }


        public T Get<T>(string id, string language) where T : class
        {
            return GetAsync<T>(id, language).WaitAndUnwrapException();
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

            var document = (await ContainerProvider.Get(ContainerConstants.Content).FindAsync(Builders<Document>.Filter.Eq(d => d.Id, id))).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var contentType = ContentTypeRepository.Get(document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"] as string);

            return (T)ContentDeserializer.Deserialize(document, contentType, language);
        }
    }
}
