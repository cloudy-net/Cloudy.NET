using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Methods
{
    public interface IContentGetter
    {
        Task<object> GetAsync(string contentTypeId, params object[] keyValues);
        Task<T> GetAsync<T>(params object[] keyValues) where T : class;
    }
}
