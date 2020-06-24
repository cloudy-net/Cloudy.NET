using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using System;
using System.Collections.Generic;
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

        public async Task<IContent> GetAsync(string contentTypeId, string language)
        {
            var singleton = SingletonProvider.Get(contentTypeId);

            if (singleton == null)
            {
                return null;
            }

            return await ContentGetter.GetAsync(contentTypeId, singleton.Id, language).ConfigureAwait(false);
        }

        public T Get<T>(string language) where T : class
        {
            var singleton = SingletonProvider.Get<T>();

            if (singleton == null)
            {
                return null;
            }

            return ContentGetter.Get<T>(singleton.Id, language);
        }
    }
}
