using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DataMethods
{
    public interface IContentUpdater
    {
        Task UpdateAsync(object content);
    }
}
