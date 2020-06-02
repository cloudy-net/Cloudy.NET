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
        IDocumentDeleter DocumentDeleter { get; }

        public ContainerSpecificContentDeleter(IDocumentDeleter documentDeleter)
        {
            DocumentDeleter = documentDeleter;
        }

        public void Delete(string id, string container)
        {
            DeleteAsync(id, container).WaitAndUnwrapException();
        }

        public async Task DeleteAsync(string id, string container)
        {
            await DocumentDeleter.DeleteAsync(container, id);
        }
    }
}
