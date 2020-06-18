using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public class CachedDocumentRepository : IDocumentGetter, IDocumentCreator, IDocumentUpdater, IDocumentFinder, IDocumentDeleter, IDocumentLister
    {
        static IDictionary<string, IDictionary<string, Document>> Documents { get; } = new Dictionary<string, IDictionary<string, Document>>();

        IServiceProvider ServiceProvider { get; set; }
        IDataSource DataSource { get; }

        public CachedDocumentRepository(IServiceProvider serviceProvider, IDataSource inMemoryDataSource)
        {
            ServiceProvider = serviceProvider;
            DataSource = inMemoryDataSource;
        }

        public async Task Create(string container, Document document)
        {
            await DataSource.CreateDocumentAsync(container, document).ConfigureAwait(false);

            if (!Documents.ContainsKey(container))
            {
                Documents[container] = (await DataSource.ListAsync(container).ConfigureAwait(false)).ToDictionary(d => d.Id, d => d);
            }

            Documents[container][document.Id] = document;
        }

        public IDocumentFinderQueryBuilder Find(string container)
        {
            var builder = (IDocumentFinderQueryBuilder)ServiceProvider.GetService(typeof(IDocumentFinderQueryBuilder));

            builder.Container = container;

            return builder;
        }

        public async Task<Document> GetAsync(string container, string id)
        {
            if (!Documents.ContainsKey(container))
            {
                Documents[container] = (await DataSource.ListAsync(container).ConfigureAwait(false)).ToDictionary(d => d.Id, d => d);
            }

            if (!Documents[container].ContainsKey(id))
            {
                return null;
            }

            return Documents[container][id];
        }

        public async Task UpdateAsync(string container, string id, Document document)
        {
            if (!Documents.ContainsKey(container))
            {
                Documents[container] = (await DataSource.ListAsync(container).ConfigureAwait(false)).ToDictionary(d => d.Id, d => d);
            }

            await DataSource.UpdateAsync(container, id, document).ConfigureAwait(false);

            Documents[container][id] = document;
        }

        public async Task DeleteAsync(string container, string id)
        {
            if (!Documents.ContainsKey(container))
            {
                Documents[container] = (await DataSource.ListAsync(container).ConfigureAwait(false)).ToDictionary(d => d.Id, d => d);
            }

            await DataSource.DeleteAsync(container, id).ConfigureAwait(false);

            Documents[container].Remove(id);
        }

        public async Task<IEnumerable<Document>> ListAsync(string container)
        {
            if (!Documents.ContainsKey(container))
            {
                Documents[container] = (await DataSource.ListAsync(container).ConfigureAwait(false)).ToDictionary(d => d.Id, d => d);
            }

            return Documents[container].Values.ToList().AsReadOnly();
        }
    }
}