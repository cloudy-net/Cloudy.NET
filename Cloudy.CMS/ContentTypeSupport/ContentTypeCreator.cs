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
        IAssemblyProvider AssemblyProvider { get; }

        public ContentTypeCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<ContentTypeDescriptor> Create()
        {
            var types = AssemblyProvider
                    .GetAll()
                    .SelectMany(a => a.Types)
                    .Where(t => t.GetTypeInfo().GetCustomAttribute<ContentTypeAttribute>() != null);

            var result = new List<ContentTypeDescriptor>();

            foreach (var type in types)
            {
                var contentTypeAttribute = type.GetTypeInfo().GetCustomAttribute<ContentTypeAttribute>();

                if (!typeof(IContent).IsAssignableFrom(type))
                {
                    throw new ContentTypeDoesNotImplementIContentException(type, contentTypeAttribute.Id);
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
