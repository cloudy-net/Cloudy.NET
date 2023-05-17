using Cloudy.NET.ContextSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.NET.SingletonSupport
{
    public record SingletonGetter(IContextCreator ContextCreator) : ISingletonGetter
    {
        public async Task<T> Get<T>() where T : class
        {
            return (T)await Get(typeof(T)).ConfigureAwait(false);
        }

        public async Task<object> Get(Type type)
        {
            var context = ContextCreator.CreateFor(type);
            var queryable = (IQueryable)context.GetDbSet(type);
            return await queryable.Cast<object>().FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}