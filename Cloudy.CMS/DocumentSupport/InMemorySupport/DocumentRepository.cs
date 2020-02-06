using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.InMemorySupport
{
    public class DocumentRepository : IDocumentGetter, IDocumentCreator, IDocumentUpdater, IDocumentFinder, IDocumentDeleter
    {
        public static IDictionary<string, IDictionary<string, Document>> Documents { get; } = new Dictionary<string, IDictionary<string, Document>>();

        IServiceProvider ServiceProvider { get; set; }

        public DocumentRepository(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public Task Create(string container, Document document)
        {
            if (!Documents.ContainsKey(container))
            {
                Documents[container] = new Dictionary<string, Document>();
            }

            Documents[container][document.Id] = document;

            return Task.CompletedTask;
        }

        public IDocumentFinderQueryBuilder Find(string container)
        {
            var builder = (IDocumentFinderQueryBuilder)ServiceProvider.GetService(typeof(IDocumentFinderQueryBuilder));

            builder.Container = container;

            return builder;
        }

        public Task<Document> GetAsync(string container, string id)
        {
            if (!Documents.ContainsKey(container))
            {
                return Task.FromResult<Document>(null);
            }

            if (!Documents[container].ContainsKey(id))
            {
                return Task.FromResult<Document>(null);
            }

            return Task.FromResult(Documents[container][id]);
        }

        public Task UpdateAsync(string container, string id, Document document)
        {
            if (!Documents.ContainsKey(container))
            {
                Documents[container] = new Dictionary<string, Document>();
            }

            Documents[container][id] = document;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(string container, string id)
        {
            if (!Documents.ContainsKey(container))
            {
                return Task.CompletedTask;
            }

            Documents[container].Remove(id);

            return Task.CompletedTask;
        }
    }
}