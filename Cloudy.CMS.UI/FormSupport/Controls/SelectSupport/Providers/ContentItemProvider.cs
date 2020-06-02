using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DocumentSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public class ContentItemProvider : IItemProvider
    {
        public IContentTypeProvider ContentTypeProvider { get; }
        public IDocumentFinder DocumentFinder { get; }

        public ContentItemProvider(IContentTypeProvider contentTypeProvider, IDocumentFinder documentFinder)
        {
            ContentTypeProvider = contentTypeProvider;
            DocumentFinder = documentFinder;
        }

        public async Task<IEnumerable<Item>> GetAll(string type)
        {
            var result = new List<Item>();

            var contentType = ContentTypeProvider.Get(type);

            foreach (var content in await DocumentFinder.Find(contentType.Container).GetResultAsync().ConfigureAwait(false))
            {
                result.Add(new Item(content.Id, content.Id));
            }

            return result.AsReadOnly();
        }
    }
}
