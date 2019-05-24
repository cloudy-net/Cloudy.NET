using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentUpdater
    {
        void Update(IContent content);
        Task UpdateAsync(IContent content);
    }
}
