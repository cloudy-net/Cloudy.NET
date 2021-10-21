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
        IContentTypeProvider ContentTypeProvider { get; }
        IContentGetter ContentGetter { get; }
        IContentInserter ContentInserter { get; }
        IContentInstanceCreator ContentInstanceCreator { get; }
        IPrimaryKeySetter PrimaryKeySetter { get; }

        public SingletonInserter(ISingletonProvider singletonProvider, IContentTypeProvider contentTypeProvider, IContentGetter contentGetter, IContentInserter contentInserter, IContentInstanceCreator contentInstanceCreator, IPrimaryKeySetter primaryKeySetter)
        {
            PrimaryKeySetter = primaryKeySetter;
            SingletonProvider = singletonProvider;
            ContentTypeProvider = contentTypeProvider;
            ContentGetter = contentGetter;
            ContentInserter = contentInserter;
            ContentInstanceCreator = contentInstanceCreator;
        }

        public async Task InitializeAsync()
        {
            foreach(var singleton in SingletonProvider.GetAll())
            {
                var content = await ContentGetter.GetAsync(singleton.ContentTypeId, singleton.KeyValues.ToArray());
                var contentType = ContentTypeProvider.Get(singleton.ContentTypeId);

                if (content != null)
                {
                    continue;
                }

                content = ContentInstanceCreator.Create(contentType);

                PrimaryKeySetter.Set(singleton.KeyValues, content);

                await ContentInserter.InsertAsync(content).ConfigureAwait(false);
            }
        }
    }
}
