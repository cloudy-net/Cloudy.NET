using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.UI.FormSupport.Controls.SelectSupport;
using Cloudy.CMS.DocumentSupport.FileSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cloudy.CMS.UI.IdentitySupport;

namespace SakraLyft
{
    [ItemProvider("user")]
    public class UserItemProvider : IItemProvider
    {
        public IContentTypeProvider ContentTypeProvider { get; }
        public IDocumentFinder DocumentFinder { get; }
        public IContentGetter ContentGetter { get; }
        public IContentDeserializer ContentDeserializer { get; }

        public UserItemProvider(IContentTypeProvider contentTypeProvider, IDocumentFinder documentFinder, IContentGetter contentGetter, IContentDeserializer contentDeserializer)
        {
            ContentTypeProvider = contentTypeProvider;
            DocumentFinder = documentFinder;
            ContentGetter = contentGetter;
            ContentDeserializer = contentDeserializer;
        }

        public async Task<ItemResponse> Get(string type, string value)
        {
            var user = await ContentGetter.GetAsync<User>(value, null).ConfigureAwait(false);

            if (user == null)
            {
                return null;
            }

            return new ItemResponse(GetItem(user), null);
        }

        public async Task<IEnumerable<Item>> GetAll(string type, ItemQuery query)
        {
            var result = new List<Item>();

            var contentType = ContentTypeProvider.Get(typeof(User));

            foreach (var document in await DocumentFinder.Find(contentType.Container).GetResultAsync().ConfigureAwait(false))
            {
                if (document.GlobalFacet.Interfaces[nameof(IContent)].Properties[nameof(IContent.ContentTypeId)] as string != type)
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
