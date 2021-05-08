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

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport.Providers
{
    [ItemProvider("user")]
    public class UserItemProvider : IItemProvider
    {
        public IContentTypeProvider ContentTypeProvider { get; }
        public IContentFinder ContentFinder { get; }
        public IContentGetter ContentGetter { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public UserItemProvider(IContentTypeProvider contentTypeProvider, IContentFinder contentFinder, IContentGetter contentGetter, IPrimaryKeyGetter primaryKeyGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentFinder = contentFinder;
            ContentGetter = contentGetter;
            PrimaryKeyGetter = primaryKeyGetter;
        }

        public async Task<ItemResponse> Get(string type, string value)
        {
            var user = await ContentGetter.GetAsync<User>(value).ConfigureAwait(false);

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
                if (!contentType.Type.IsAssignableFrom(content.GetType()))
                {
                    continue;
                }

                result.Add(GetItem(content));
            }

            return result.AsReadOnly();
        }

        Item GetItem(object content)
        {
            var id = "{" + string.Join(",", PrimaryKeyGetter.Get(content)) + "}";
            return new Item((content as INameable)?.Name ?? id, null, id, (content as IImageable)?.Image, content is IHierarchical);
        }
    }
}
