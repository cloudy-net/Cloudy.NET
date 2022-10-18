using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContextSupport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.ContentTypeSupport
{
    public record ContentTypeCreator(IContextDescriptorProvider ContextDescriptorProvider) : IContentTypeCreator
    {
        public IEnumerable<ContentTypeDescriptor> Create()
        {
            var types = ContextDescriptorProvider.GetAll().SelectMany(c => c.DbSets.Select(p => p.Type)).ToList();

            var result = new List<ContentTypeDescriptor>();

            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                result.Add(new ContentTypeDescriptor(
                    type.Name,
                    type,
                    type.IsAssignableTo(typeof(INameable)),
                    type.IsAssignableTo(typeof(IImageable)),
                    type.IsAssignableTo(typeof(IRoutable))
                ));
            }

            return result;
        }
    }
}
