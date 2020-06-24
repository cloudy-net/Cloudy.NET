using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentGetter : IContentGetter
    {
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContentTypeProvider ContentTypeProvider { get; }

        public ContentGetter(IContainerSpecificContentGetter containerSpecificContentGetter, IContentTypeProvider contentTypeProvider)
        {
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContentTypeProvider = contentTypeProvider;
        }

        public T Get<T>(string id, string language) where T : class
        {
            return ContainerSpecificContentGetter.Get<T>(id, language, ContainerConstants.Content);
        }

        public async Task<T> GetAsync<T>(string id, string language) where T : class
        {
            return await ContainerSpecificContentGetter.GetAsync<T>(id, language, ContainerConstants.Content);
        }

        public async Task<IContent> GetAsync(string contentTypeId, string id, string language)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            return await ContainerSpecificContentGetter.GetAsync(id, language, contentType.Container).ConfigureAwait(false);
        }
    }
}
