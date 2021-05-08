using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentGetter
    {
        Task<IContent> GetAsync(string contentTypeId, params object[] keyValues);
        Task<T> GetAsync<T>(params object[] keyValues) where T : class;
    }
}
