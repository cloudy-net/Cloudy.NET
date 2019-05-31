using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public interface IContainerSpecificContentCreator
    {
        void Create(IContent content, string container);
        Task CreateAsync(IContent content, string container);
    }
}
