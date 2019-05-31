using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public interface IContainerSpecificContentGetter
    {
        T Get<T>(string id, string language, string container) where T : class;
        Task<T> GetAsync<T>(string id, string language, string container) where T : class;
    }
}
