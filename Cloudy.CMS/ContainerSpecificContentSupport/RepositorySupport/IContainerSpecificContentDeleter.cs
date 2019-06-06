using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public interface IContainerSpecificContentDeleter
    {
        void Delete(string id, string container);
        Task DeleteAsync(string id, string container);
    }
}
