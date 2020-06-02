using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentUpdater : IContentUpdater
    {
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }

        public ContentUpdater(IContainerSpecificContentUpdater containerSpecificContentUpdater)
        {
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
        }

        public void Update(IContent content)
        {
            ContainerSpecificContentUpdater.Update(content, ContainerConstants.Content);
        }

        public async Task UpdateAsync(IContent content)
        {
            await ContainerSpecificContentUpdater.UpdateAsync(content, ContainerConstants.Content);
        }
    }
}
