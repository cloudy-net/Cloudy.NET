using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class AncestorLinkProvider : IAncestorLinkProvider
    {
        IDocumentFinder DocumentFinder { get; }

        public AncestorLinkProvider(IDocumentFinder documentFinder)
        {
            DocumentFinder = documentFinder;
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
                var document = (await DocumentFinder.Find(ContainerConstants.Content).WhereEquals<IContent, string>(x => x.Id, id).WhereExists<IHierarchical, string>(x => x.ParentId).Select<IHierarchical, string>(x => x.ParentId).GetResultAsync()).FirstOrDefault();

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
