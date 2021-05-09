using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContentInserter : IContentInserter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDbSetProvider DbSetProvider { get; }

        public ContentInserter(IContentTypeProvider contentTypeProvider, IDbSetProvider dbSetProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            DbSetProvider = dbSetProvider;
        }

        public async Task InsertAsync(object content)
        {
            var contentType = ContentTypeProvider.Get(content.GetType());
            var dbSet = DbSetProvider.Get(contentType.Type);
            await dbSet.AddAsync(content).ConfigureAwait(false);
        }
    }
}
