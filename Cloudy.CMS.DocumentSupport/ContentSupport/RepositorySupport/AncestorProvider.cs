using Cloudy.CMS.ContentTypeSupport;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class AncestorProvider : IAncestorProvider
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDocumentFinder DocumentFinder { get; }
        IContentDeserializer ContentDeserializer { get; }

        public AncestorProvider(IContentTypeProvider contentTypeProvider, IDocumentFinder documentFinder, IContentDeserializer contentDeserializer)
        {
            ContentTypeProvider = contentTypeProvider;
            DocumentFinder = documentFinder;
            ContentDeserializer = contentDeserializer;
        }

        public async Task<IEnumerable<IContent>> GetAncestorsAsync(IContent content)
        {
            var result = new List<IContent>();

            var position = content;
            var contentType = ContentTypeProvider.Get(content.ContentTypeId);

            while (true)
            {
                var document = (await DocumentFinder.Find(contentType.Container).WhereEquals<IContent, string>(x => x.Id, ((IHierarchical)position).ParentId).WhereExists<IHierarchical, string>(x => x.ParentId).GetResultAsync().ConfigureAwait(false)).FirstOrDefault();

                if (document == null)
                {
                    break;
                }

                contentType = ContentTypeProvider.Get((string)document.GlobalFacet.Interfaces["IContent"].Properties["ContentTypeId"]);
                position = ContentDeserializer.Deserialize(document, contentType, null);

                result.Add(position);

                if(((IHierarchical)position).ParentId == null)
                {
                    break;
                }
            }

            return result;
        }
    }
}
