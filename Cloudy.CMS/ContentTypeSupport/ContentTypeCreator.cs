using Poetry.ComponentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeCreator : IContentTypeCreator
    {
        IComponentProvider ComponentProvider { get; }

        public ContentTypeCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<ContentTypeDescriptor> Create()
        {
            var types = ComponentProvider
                    .GetAll()
                    .SelectMany(a => a.Assembly.Types)
                    .Where(a => typeof(IContent).IsAssignableFrom(a));

            var result = new List<ContentTypeDescriptor>();

            foreach (var type in types)
            {
                var contentTypeAttribute = type.GetTypeInfo().GetCustomAttribute<ContentTypeAttribute>();

                if (contentTypeAttribute == null)
                {
                    continue;
                }

                if (type.IsAbstract)
                {
                    continue;
                }

                var container = type.GetTypeInfo().GetCustomAttribute<ContainerAttribute>()?.Id ?? ContainerConstants.Content;

                result.Add(new ContentTypeDescriptor(contentTypeAttribute.Id, type, container));
            }

            return result;
        }
    }
}
