using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentChildrenCounter
    {
        Task<int> CountChildrenForAsync(string container, string id);
    }
}
