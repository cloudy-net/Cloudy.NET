using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentGetter
    {
        Task<IContent> GetAsync(string contentTypeId, string id);
        Task<T> GetAsync<T>(string id) where T : class;
    }
}
