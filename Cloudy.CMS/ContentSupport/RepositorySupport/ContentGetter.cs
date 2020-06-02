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

        public ContentGetter(IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        public T Get<T>(string id, string language) where T : class
        {
            return ContainerSpecificContentGetter.Get<T>(id, language, ContainerConstants.Content);
        }

        public async Task<T> GetAsync<T>(string id, string language) where T : class
        {
            return await ContainerSpecificContentGetter.GetAsync<T>(id, language, ContainerConstants.Content);
        }
    }
}
