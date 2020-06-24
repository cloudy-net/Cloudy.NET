using Cloudy.CMS.ComposableSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RuntimeSupport
{
    public class ContentInstanceCreator : IContentInstanceCreator
    {
        IEnumerable<IContentInstanceInitializer> Initializers { get; }

        public ContentInstanceCreator(IComposableProvider composableProvider)
        {
            Initializers = composableProvider.GetAll<IContentInstanceInitializer>().ToList().AsReadOnly();
        }

        public IContent Create(ContentTypeDescriptor contentType)
        {
            var content = (IContent)Activator.CreateInstance(contentType.Type);

            foreach(var initializer in Initializers)
            {
                initializer.Initialize(content, contentType);
            }

            return content;
        }
    }
}
