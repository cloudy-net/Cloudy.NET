using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public class SingletonCreator : ISingletonCreator
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public SingletonCreator(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<SingletonDescriptor> Create()
        {
            var result = new List<SingletonDescriptor>();

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                var singletonAttribute = contentType.Type.GetCustomAttribute<SingletonAttribute>();

                if(singletonAttribute == null)
                {
                    continue;
                }

                result.Add(new SingletonDescriptor(contentType.Name, contentType.Type));
            }

            return result.AsReadOnly();
        }
    }
}
