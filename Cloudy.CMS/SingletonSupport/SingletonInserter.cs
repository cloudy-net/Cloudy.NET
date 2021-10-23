using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport.RuntimeSupport;
using System.Linq;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonInserter : IInitializer
    {
        ISingletonProvider SingletonProvider { get; }
        ISingletonGetter SingletonGetter { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentGetter ContentGetter { get; }
        IContentInstanceCreator ContentInstanceCreator { get; }
        IContentCreator ContentCreator { get; }

        public SingletonInserter(ISingletonProvider singletonProvider, ISingletonGetter singletonGetter, IContentTypeProvider contentTypeProvider, IContentGetter contentGetter, IContentInstanceCreator contentInstanceCreator, IContentCreator contentCreator)
        {
            SingletonProvider = singletonProvider;
            SingletonGetter = singletonGetter;
            ContentTypeProvider = contentTypeProvider;
            ContentGetter = contentGetter;
            ContentInstanceCreator = contentInstanceCreator;
            ContentCreator = contentCreator;
        }

        public async Task InitializeAsync()
        {
            foreach (var singleton in SingletonProvider.GetAll())
            {
                var content = await SingletonGetter.GetAsync(singleton.ContentTypeId).ConfigureAwait(false);
                var contentType = ContentTypeProvider.Get(singleton.ContentTypeId);

                if (content != null)
                {
                    continue;
                }

                content = ContentInstanceCreator.Create(contentType);

                await ContentCreator.CreateAsync(content).ConfigureAwait(false);
            }
        }
    }
}
