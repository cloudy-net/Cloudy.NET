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
    public class AncestorLinkProvider : IAncestorLinkProvider
    {
        IContainerProvider ContainerProvider { get; }

        public AncestorLinkProvider(IContainerProvider containerProvider)
        {
            ContainerProvider = containerProvider;
        }

        public IEnumerable<string> GetAncestorLinks(string id)
        {
            return GetAncestorLinksAsync(id).WaitAndUnwrapException();
        }

        public async Task<IEnumerable<string>> GetAncestorLinksAsync(string id)
        {
            var result = new List<string>();

            while (true)
            {
                var parent = (await ContainerProvider.Get(ContainerConstants.Content).FindAsync(
                    Builders<Document>.Filter.Eq(d => d.GlobalFacet.Interfaces["IContent"].Properties["Id"], id),
                    new FindOptions<Document, Document> { Projection = Builders<Document>.Projection.Include(d => d.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"]) }
                ))
                .FirstOrDefault();

                if (parent == null || parent.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"] == null)
                {
                    break;
                }

                result.Add(parent.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"] as string);

                id = parent.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"] as string;
            }

            return result;
        }
    }
}
