using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Methods
{
    public interface IContentChildrenCounter
    {
        Task<int> CountChildrenForAsync(params object[] keyValues);
    }
}
