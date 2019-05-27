using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Poetry.ComponentSupport;
using Poetry.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.SingletonSupport
{
    public class SingletonCreator : IInitializer
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentGetter ContentGetter { get; }
        IContentInserter ContentInserter { get; }

        public SingletonCreator(IContentTypeProvider contentTypeProvider, IContentGetter contentGetter, IContentInserter contentInserter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentGetter = contentGetter;
            ContentInserter = contentInserter;
        }

        public void Initialize()
        {
            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                var singletonAttribute = contentType.Type.GetCustomAttribute<SingletonAttribute>();

                if(singletonAttribute == null)
                {
                    continue;
                }

                var id = singletonAttribute.Id;

                var content = ContentGetter.Get<IContent>(id, null);

                if (content != null)
                {
                    if(content.ContentTypeId != contentType.Id)
                    {
                        throw new SingletonWithIdIsOfWrongType(id, contentType, content.GetType(), content.ContentTypeId);
                    }

                    return;
                }

                content = (IContent)Activator.CreateInstance(contentType.Type);

                content.Id = id;

                ContentInserter.Insert(content);
            }
        }
    }
}
