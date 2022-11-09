using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudy.CMS.Naming;
using System.Text.Json;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonGetter : ISingletonGetter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }

        public SingletonGetter(IContentTypeProvider contentTypeProvider, IContextCreator contextCreator)
        {
            ContentTypeProvider = contentTypeProvider;
            ContextCreator = contextCreator;
        }

        public async Task<object> GetAsync(Type type)
        {
            using var context = ContextCreator.CreateFor(type);
            var dbSet = (IQueryable)context.GetDbSet(type);

            dbSet = dbSet.Cast<object>().Take(1);

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                return instance;
            }

            return null;
        }
    }
}
