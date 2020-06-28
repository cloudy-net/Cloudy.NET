using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentGetter
    {
        Task<IContent> GetAsync(string contentTypeId, string id, string language);
        Task<T> GetAsync<T>(string id, string language) where T : class;
    }
}
