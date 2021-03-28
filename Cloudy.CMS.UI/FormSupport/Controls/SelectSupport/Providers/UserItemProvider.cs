using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.Controls.SelectSupport;
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
        public IContentFinder ContentFinder { get; }
        public IContentGetter ContentGetter { get; }

        public UserItemProvider(IContentTypeProvider contentTypeProvider, IContentFinder contentFinder, IContentGetter contentGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentFinder = contentFinder;
            ContentGetter = contentGetter;
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

            foreach (var content in await ContentFinder.FindInContainer(contentType.Container).GetResultAsync().ConfigureAwait(false))
            {
                if (content.ContentTypeId != type)
                {
                    continue;
                }

                result.Add(GetItem(content));
            }

            return result.AsReadOnly();
        }

        Item GetItem(IContent content) => new Item((content as INameable)?.Name ?? content.Id, null, content.Id, (content as IImageable)?.Image, content is IHierarchical);
    }
}
