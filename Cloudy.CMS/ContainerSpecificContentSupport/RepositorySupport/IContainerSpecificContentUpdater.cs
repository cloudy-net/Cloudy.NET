using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public interface IContainerSpecificContentUpdater
    {
        void Update(IContent content, string container);
        Task UpdateAsync(IContent content, string container);
    }
}
