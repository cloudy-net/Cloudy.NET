using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.DataMethods;
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

        public async Task<object> GetAsync(Type type)
        {
            return (await ContentFinder.Find(type).GetResultAsync().ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>() where T : class
        {
            return (T)(await ContentFinder.Find(typeof(T)).GetResultAsync().ConfigureAwait(false)).FirstOrDefault();
        }
    }
}
