using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IDbSetWrapper
    {
        object DbSet { get; }
        Task<object> FindAsync(object[] keyValues);
        Task AddAsync(object entity);
    }
}