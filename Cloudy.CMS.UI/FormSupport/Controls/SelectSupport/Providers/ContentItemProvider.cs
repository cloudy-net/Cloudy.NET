using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.DocumentSupport.FileSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    [ItemProvider("content")]
    public class ContentItemProvider : IItemProvider
    {
        public IContentTypeProvider ContentTypeProvider { get; }
        public IDocumentFinder DocumentFinder { get; }
        public IContentGetter ContentGetter { get; }
        public IContentDeserializer ContentDeserializer { get; }
        public IAncestorProvider AncestorProvider { get; }

        public ContentItemProvider(IContentTypeProvider contentTypeProvider, IDocumentFinder documentFinder, IContentGetter contentGetter, IContentDeserializer contentDeserializer, IAncestorProvider ancestorProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            DocumentFinder = documentFinder;
            ContentGetter = contentGetter;
            ContentDeserializer = contentDeserializer;
            AncestorProvider = ancestorProvider;
        }

        public async Task<ItemResponse> Get(string type, string value)
        {
            var content = await ContentGetter.GetAsync(type, value, null).ConfigureAwait(false);

            if(content == null)
            {
                return null;
            }

            var ancestors = await AncestorProvider.GetAncestorsAsync(content).ConfigureAwait(false);

            return new ItemResponse(GetItem(content), ancestors.Select(a => new ItemParent((a as INameable)?.Name ?? a.Id, a.Id)).ToList().AsReadOnly());
        }

        public async Task<IEnumerable<Item>> GetAll(string type, ItemQuery query)
        {
            var result = new List<Item>();

            var contentType = ContentTypeProvider.Get(type);

            foreach (var document in await DocumentFinder.Find(contentType.Container).GetResultAsync().ConfigureAwait(false))
            {
                if(document.GlobalFacet.Interfaces[nameof(IContent)].Properties[nameof(IContent.ContentTypeId)] as string != type)
                {
                    continue;
                }
                
                var content = ContentDeserializer.Deserialize(document, contentType, null);

                result.Add(GetItem(content));
            }

            return result.AsReadOnly();
        }

        Item GetItem(IContent content) => new Item((content as INameable)?.Name ?? content.Id, null, content.Id, (content as IImageable)?.Image, content is IHierarchical);
    }
}
