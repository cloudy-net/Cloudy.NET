using MongoDB.Driver;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport
{
    public class ContainerSpecificContentDeleter : IContainerSpecificContentDeleter
    {
        IContainerProvider ContainerProvider { get; }

        public ContainerSpecificContentDeleter(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public void Delete(string id, string container)
        {
            DeleteAsync(id, container).WaitAndUnwrapException();
        }

        public async Task DeleteAsync(string id, string container)
        {
            await ContainerProvider.Get(container).FindOneAndDeleteAsync(Builders<Document>.Filter.Eq(d => d.Id, id));
        }
    }
}
