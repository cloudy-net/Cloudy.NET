using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DocumentDeleter : IDocumentDeleter
    {
        IContainerProvider ContainerProvider { get; }

        public DocumentDeleter(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public async Task DeleteAsync(string container, string id)
        {
            await ContainerProvider.Get(container).FindOneAndDeleteAsync(Builders<Document>.Filter.Eq(d => d.Id, id));
        }
    }
}
