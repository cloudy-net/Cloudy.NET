using MongoDB.Driver;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;

namespace Cloudy.CMS.Core.ContentSupport.RepositorySupport
{
    public class ContentDeleter : IContentDeleter
    {
        IContainerSpecificContentDeleter ContainerSpecificContentDeleter { get; }

        public ContentDeleter(IContainerSpecificContentDeleter containerSpecificContentDeleter)
        {
            ContainerSpecificContentDeleter = containerSpecificContentDeleter;
        }

        public void Delete(IContent content)
        {
            ContainerSpecificContentDeleter.Delete(content, ContainerConstants.Content);
        }

        public async Task DeleteAsync(IContent content)
        {
            await ContainerSpecificContentDeleter.DeleteAsync(content, ContainerConstants.Content);
        }
    }
}
