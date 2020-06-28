using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentDeleter : IContentDeleter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IDocumentDeleter DocumentDeleter { get; }

        public ContentDeleter(IContentTypeProvider contentTypeProvider, IDocumentDeleter documentDeleter)
        {
            ContentTypeProvider = contentTypeProvider;
            DocumentDeleter = documentDeleter;
        }

        public async Task DeleteAsync<T>(string id) where T : class, IContent
        {
            var contentType = ContentTypeProvider.Get(typeof(T));

            if (contentType == null)
            {
                throw new TypeNotRegisteredContentTypeException(typeof(T));
            }

            await DocumentDeleter.DeleteAsync(contentType.Container, id);
        }

        public async Task DeleteAsync(string contentTypeId, string id)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            if (contentType == null)
            {
                throw new ArgumentException(nameof(contentTypeId));
            }

            await DocumentDeleter.DeleteAsync(contentType.Container, id);
        }
    }
}
