using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.Core.ContentSupport.RepositorySupport
{
    public interface IContentDeleter
    {
        void Delete(IContent content);
        Task DeleteAsync(IContent content);
    }
}
