using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentGetter : IContentGetter
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public ContentGetter(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public async Task<object> GetAsync(string contentTypeId, params object[] keyValues)
        {

            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            var contentType = ContentTypeProvider.Get(typeof(T));

            return (T)await GetAsync(contentType.Id, keyValues);
        }
    }
}
