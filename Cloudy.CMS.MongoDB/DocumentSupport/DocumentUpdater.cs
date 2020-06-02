using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DocumentUpdater : IDocumentUpdater
    {
        IContainerProvider ContainerProvider { get; }

        public DocumentUpdater(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public async Task UpdateAsync(string container, string id, Document document)
        {
            await ContainerProvider.Get(container).UpdateOneAsync(Builders<Document>.Filter.Eq(d => d.Id, id), Builders<Document>.Update.Set(d => d.GlobalFacet, document.GlobalFacet));
        }
    }
}
