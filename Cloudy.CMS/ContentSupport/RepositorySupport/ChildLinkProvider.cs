using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using MongoDB.Driver;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ChildLinkProvider : IChildLinkProvider
    {
        IContainerProvider ContainerProvider { get; }

        public ChildLinkProvider(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public IEnumerable<string> GetChildLinks(string id)
        {
            return GetChildLinksAsync(id).WaitAndUnwrapException();
        }

        public async Task<IEnumerable<string>> GetChildLinksAsync(string id)
        {
            return (await ContainerProvider.Get(ContainerConstants.Content).FindAsync(Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"], id)))
            .ToList()
            .Select(d => d.Id)
            .ToList()
            .AsReadOnly();
        }
    }
}
