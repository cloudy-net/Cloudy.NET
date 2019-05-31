using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentCreator : IContentCreator
    {
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        IContainerProvider ContainerProvider { get; }
        IIdGenerator IdGenerator { get; }
        IContentTypeProvider ContentTypeRepository { get; }
        IContentSerializer ContentSerializer { get; }

        public ContentCreator(IContainerSpecificContentCreator containerSpecificContentCreator)
        {
            ContainerSpecificContentCreator = containerSpecificContentCreator;
        }

        public void Create(IContent content)
        {
            ContainerSpecificContentCreator.Create(content, ContainerConstants.Content);
        }

        public async Task CreateAsync(IContent content)
        {
            await ContainerSpecificContentCreator.CreateAsync(content, ContainerConstants.Content);
        }
    }
}
