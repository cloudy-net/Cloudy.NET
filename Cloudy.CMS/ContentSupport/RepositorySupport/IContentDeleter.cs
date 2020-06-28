using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentDeleter
    {
        Task DeleteAsync(string contentTypeId, string id);
        Task DeleteAsync<T>(string id) where T : class, IContent;
    }
}
