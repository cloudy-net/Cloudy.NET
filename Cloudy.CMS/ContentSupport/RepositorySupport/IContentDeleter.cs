using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.Core.ContentSupport.RepositorySupport
{
    public interface IContentDeleter
    {
        void Delete(string id);
        Task DeleteAsync(string id);
    }
}
