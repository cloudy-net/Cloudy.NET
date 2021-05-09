using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContentGetter : IContentGetter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDbSetProvider DbSetProvider { get; }

        public ContentGetter(IContentTypeProvider contentTypeProvider, IDbSetProvider dbSetProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            DbSetProvider = dbSetProvider;
        }

        public async Task<object> GetAsync(string contentTypeId, params object[] keyValues)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);
            var dbSet = DbSetProvider.Get(contentType.Type);
            return await dbSet.FindAsync(keyValues).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            var dbSet = DbSetProvider.Get(typeof(T));
            return (T)await dbSet.FindAsync(keyValues).ConfigureAwait(false);
        }
    }
}
