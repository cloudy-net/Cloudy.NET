using Cloudy.CMS.DocumentSupport.CacheSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.InMemorySupport
{
    /// <summary>
    /// The data source for the in memory implementation of the document store.
    /// As an in memory anything always starts out empty, and is cached on a second level, it does nothing at all.
    /// </summary>
    public class InMemoryDataSource : IDataSource
    {
        public Task CreateDocumentAsync(string container, Document document)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string container, string id)
        {
            return Task.CompletedTask;
        }

        public Task<Document> GetAsync(string container, string id)
        {
            return Task.FromResult<Document>(null);
        }

        public Task UpdateAsync(string container, string id, Document document)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Document>> ListAsync(string container)
        {
            return Task.FromResult(Enumerable.Empty<Document>());
        }
    }
}
