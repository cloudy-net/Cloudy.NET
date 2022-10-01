using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Methods
{
    public interface IContentMover
    {
        Task MoveAsync(object content, params object[] keyValues);
    }
}