using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentInserter
    {
        void Insert(IContent content);
        Task InsertAsync(IContent content);
    }
}