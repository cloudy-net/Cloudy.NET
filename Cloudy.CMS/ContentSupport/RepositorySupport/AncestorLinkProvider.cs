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
                var document = (await ContainerProvider.Get(ContainerConstants.Content).FindAsync(
                    Builders<Document>.Filter.And(
                        Builders<Document>.Filter.Eq(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IContent.Properties.Id"), id),
                        Builders<Document>.Filter.Exists(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IHierarchical.Properties.ParentId"))
                    ),
                    new FindOptions<Document, Document> { Projection = Builders<Document>.Projection.Include(new StringFieldDefinition<Document, string>("GlobalFacet.Interfaces.IHierarchical.Properties.ParentId")) }
                ))
                .FirstOrDefault();

                if (document == null)
                {
                    break;
                }

                var parentId = document.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"] as string;

                if(parentId == null)
                {
                    break;
                }

                result.Add(parentId);

                id = parentId;
            }

            return result;
        }
    }
}
