using Cloudy.CMS.ContextSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.SingletonSupport
{
    public record SingletonGetter(IContextCreator ContextCreator) : ISingletonGetter
    {
        public async Task<T> Get<T>() where T : class
        {
            var context = ContextCreator.CreateFor(typeof(T));
            var queryable = (IQueryable)context.GetDbSet(typeof(T));
            return (T)await queryable.Cast<object>().FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}