using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonGetter : ISingletonGetter
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentFinder ContentFinder { get; }

        public SingletonGetter(IContentTypeProvider contentTypeProvider, IContentFinder contentFinder)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentFinder = contentFinder;
        }

        public async Task<object> GetAsync(string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);
            return (await ContentFinder.Find(contentType.Type).GetResultAsync().ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>() where T : class
        {
            return (T)(await ContentFinder.Find(typeof(T)).GetResultAsync().ConfigureAwait(false)).FirstOrDefault();
        }
    }
}
