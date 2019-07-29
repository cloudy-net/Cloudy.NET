using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DocumentGetter : IDocumentGetter
    {
        IContainerProvider ContainerProvider { get; }

        public DocumentGetter(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public async Task<Document> GetAsync(string container, string id)
        {
            return (await ContainerProvider.Get(container).FindAsync(Builders<Document>.Filter.Eq(d => d.Id, id))).FirstOrDefault();
        }
    }
}
