using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    [ItemProvider("content")]
    public class ContentItemProvider : IItemProvider
    {
        public IContentTypeProvider ContentTypeProvider { get; }
        public IDocumentFinder DocumentFinder { get; }
        public IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }

        public ContentItemProvider(IContentTypeProvider contentTypeProvider, IDocumentFinder documentFinder, IContainerSpecificContentGetter containerSpecificContentGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            DocumentFinder = documentFinder;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
        }

        public async Task<Item> Get(string type, string value)
        {
            var contentType = ContentTypeProvider.Get(type);
            var content = await ContainerSpecificContentGetter.GetAsync<IContent>(value, null, contentType.Container).ConfigureAwait(false);

            if(content == null)
            {
                return null;
            }

            return new Item((content as INameable)?.Name ?? content.Id, content.Id, null, new Dictionary<string, string> { });
        }

        public async Task<IEnumerable<Item>> GetAll(string type)
        {
            var result = new List<Item>();

            var contentType = ContentTypeProvider.Get(type);

            foreach (var content in await DocumentFinder.Find(contentType.Container).GetResultAsync().ConfigureAwait(false))
            {
                result.Add(new Item((content as INameable)?.Name ?? content.Id, content.Id, null, new Dictionary<string, string> { }));
            }

            return result.AsReadOnly();
        }
    }
}
