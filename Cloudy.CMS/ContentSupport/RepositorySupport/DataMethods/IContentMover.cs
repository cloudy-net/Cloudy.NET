using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DataMethods
{
    public interface IContentMover
    {
        Task MoveAsync(object content, params object[] keyValues);
    }
}