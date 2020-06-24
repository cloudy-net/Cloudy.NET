using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public class ContentTypeGroupCreator : IContentTypeGroupCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public ContentTypeGroupCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<ContentTypeGroupDescriptor> Create()
        {
            var types = AssemblyProvider
                    .GetAll()
                    .SelectMany(a => a.Types)
                    .Where(a => a.GetTypeInfo().GetCustomAttribute<ContentTypeGroupAttribute>() != null);

            var result = new List<ContentTypeGroupDescriptor>();

            foreach (var type in types)
            {
                var contentTypeAttribute = type.GetTypeInfo().GetCustomAttribute<ContentTypeGroupAttribute>();

                if (contentTypeAttribute == null)
                {
                    continue;
                }

                result.Add(new ContentTypeGroupDescriptor(contentTypeAttribute.Id, type));
            }

            return result.AsReadOnly();
        }
    }
}
