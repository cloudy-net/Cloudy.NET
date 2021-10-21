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
        ISingletonProvider SingletonProvider { get; }
        IContentGetter ContentGetter { get; }

        public SingletonGetter(ISingletonProvider singletonProvider, IContentGetter contentGetter)
        {
            SingletonProvider = singletonProvider;
            ContentGetter = contentGetter;
        }

        public async Task<object> GetAsync(string contentTypeId)
        {
            var singleton = SingletonProvider.Get(contentTypeId);

            if (singleton == null)
            {
                return null;
            }

            return await ContentGetter.GetAsync(singleton.ContentTypeId, singleton.KeyValues.ToArray()).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>() where T : class
        {
            var singleton = SingletonProvider.Get<T>();

            if (singleton == null)
            {
                return null;
            }

            return (T)await ContentGetter.GetAsync(singleton.ContentTypeId, singleton.KeyValues).ConfigureAwait(false);
        }
    }
}
