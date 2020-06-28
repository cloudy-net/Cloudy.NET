using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentInserter
    {
        Task InsertAsync(IContent content);
    }
}