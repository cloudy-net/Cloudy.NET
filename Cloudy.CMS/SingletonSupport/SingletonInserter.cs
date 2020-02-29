using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonInserter : IInitializer
    {
        ISingletonProvider SingletonProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContentGetter ContentGetter { get; }
        IContentInserter ContentInserter { get; }

        public SingletonInserter(ISingletonProvider singletonProvider, IContentTypeProvider contentTypeProvider, IContentGetter contentGetter, IContentInserter contentInserter)
        {
            SingletonProvider = singletonProvider;
            ContentTypeProvider = contentTypeProvider;
            ContentGetter = contentGetter;
            ContentInserter = contentInserter;
        }

        public void Initialize()
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

                content = (IContent)Activator.CreateInstance(contentType.Type);

                content.Id = singleton.Id;

                ContentInserter.Insert(content);
            }
        }
    }
}
