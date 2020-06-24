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

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonInserter : IInitializer
    {
        ISingletonProvider SingletonProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentGetter ContentGetter { get; }
        IContentInserter ContentInserter { get; }
        IContentInstanceCreator ContentInstanceCreator { get; }

        public SingletonInserter(ISingletonProvider singletonProvider, IContentTypeProvider contentTypeProvider, IContentGetter contentGetter, IContentInserter contentInserter, IContentInstanceCreator contentInstanceCreator)
        {
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
                var content = ContentGetter.Get<IContent>(singleton.Id, null);
                var contentType = ContentTypeProvider.Get(singleton.ContentTypeId);

                if (content != null)
                {
                    if(content.ContentTypeId != contentType.Id)
                    {
                        throw new SingletonWithIdIsOfWrongType(singleton.Id, contentType, content.GetType(), content.ContentTypeId);
                    }

                    continue;
                }

                content = ContentInstanceCreator.Create(contentType);

                content.Id = singleton.Id;

                ContentInserter.Insert(content);
            }
        }
    }
}
