using Cloudy.CMS.ContentSupport.RepositorySupport;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContentChildrenCounter : IContentChildrenCounter
    {
        public async Task<int> CountChildrenForAsync(params object[] keyValues)
        {
            return 0;
        }
    }
}