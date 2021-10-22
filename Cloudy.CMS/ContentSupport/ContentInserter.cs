using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    public class ContentInserter : IContentInserter
    {
        IDbSetProvider DbSetProvider { get; }
        IContextProvider ContextProvider { get; }

        public ContentInserter(IDbSetProvider dbSetProvider, IContextProvider contextProvider)
        {
            DbSetProvider = dbSetProvider;
            ContextProvider = contextProvider;
        }

        public async Task InsertAsync(object content)
        {
            var dbSet = DbSetProvider.Get(content.GetType());
            await dbSet.AddAsync(content).ConfigureAwait(false);
            await ContextProvider.GetFor(content.GetType()).Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
