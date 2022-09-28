using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using Cloudy.CMS.ContentSupport.RepositorySupport.DbSet;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DataMethods
{
    public class ContentDeleter : IContentDeleter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDbSetProvider DbSetProvider { get; }
        IContentGetter ContentGetter { get; }
        IContextProvider ContextProvider { get; }

        public ContentDeleter(IContentTypeProvider contentTypeProvider, IDbSetProvider dbSetProvider, IContentGetter contentGetter, IContextProvider contextProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            DbSetProvider = dbSetProvider;
            ContentGetter = contentGetter;
            ContextProvider = contextProvider;
        }

        public async Task DeleteAsync(string contentTypeId, params object[] keyValues)
        {
            var content = await ContentGetter.GetAsync(contentTypeId, keyValues).ConfigureAwait(false);
            var contentType = ContentTypeProvider.Get(contentTypeId);
            var dbSet = DbSetProvider.Get(contentType.Type);
            dbSet.Remove(content);
            var context = ContextProvider.GetFor(content.GetType());
            await context.Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
