using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IDbSetWrapper
    {
        Task<object> FindAsync(object[] keyValues);
    }
}