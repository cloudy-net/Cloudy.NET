using Cloudy.CMS.ContentSupport.RepositorySupport;
using System;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentChildrenCounter : IContentChildrenCounter
    {
        public async Task<int> CountChildrenForAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }
    }
}