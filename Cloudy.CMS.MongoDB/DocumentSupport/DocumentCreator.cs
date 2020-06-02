using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DocumentCreator : IDocumentCreator
    {
        IContainerProvider ContainerProvider { get; }

        public DocumentCreator(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public async Task Create(string container, Document document)
        {
            await ContainerProvider.Get(container).InsertOneAsync(document);
        }
    }
}
