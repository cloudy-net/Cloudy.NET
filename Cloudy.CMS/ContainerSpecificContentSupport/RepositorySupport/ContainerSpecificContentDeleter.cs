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

        public void Delete(IContent content, string container)
        {
            DeleteAsync(content, container).WaitAndUnwrapException();
        }

        public async Task DeleteAsync(IContent content, string container)
        {
            if (content.Id == null)
            {
                throw new InvalidOperationException($"This content cannot be deleted as it doesn't seem to exist (Id is null)");
            }

            await ContainerProvider.Get(container).FindOneAndDeleteAsync(Builders<Document>.Filter.Eq(d => d.Id, content.Id));
        }
    }
}
