using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ChildLinkProvider : IChildLinkProvider
    {
        IDocumentFinder DocumentFinder { get; }

        public ChildLinkProvider(IDocumentFinder documentFinder)
        {
            DocumentFinder = documentFinder;
        }

        public IEnumerable<string> GetChildLinks(string id)
        {
            return GetChildLinksAsync(id).WaitAndUnwrapException();
        }

        public async Task<IEnumerable<string>> GetChildLinksAsync(string id)
        {
            var documents = await DocumentFinder.Find(ContainerConstants.Content).WhereEquals<IHierarchical, string>(x => x.ParentId, id).GetResultAsync();

            return documents.Select(d => d.Id).ToList().AsReadOnly();
        }
    }
}
