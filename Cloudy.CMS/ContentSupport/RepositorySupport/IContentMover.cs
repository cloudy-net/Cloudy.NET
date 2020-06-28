using Cloudy.CMS.ContentSupport;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentMover
    {
        Task MoveAsync(IContent content, string id);
    }
}