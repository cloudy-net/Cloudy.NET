using Cloudy.CMS.ContentSupport.EntityFrameworkSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentGetter : IContentGetter
    {
        IDbSetProvider DbSetProvider { get; }

        public ContentGetter(IDbSetProvider dbSetProvider)
        {
            DbSetProvider = dbSetProvider;
        }

        public async Task<object> GetAsync(string contentTypeId, params object[] keyValues)
        {
            var dbSet = DbSetProvider.Get(contentTypeId);
            return await dbSet.FindAsync(keyValues).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            var dbSet = DbSetProvider.Get<T>();
            return (T)await dbSet.FindAsync(keyValues).ConfigureAwait(false);
        }
    }
}
