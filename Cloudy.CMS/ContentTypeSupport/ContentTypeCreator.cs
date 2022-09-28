using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public record ContentTypeCreator(IContextDescriptorProvider ContextDescriptorProvider) : IContentTypeCreator
    {
        public IEnumerable<ContentTypeDescriptor> Create()
        {
            var types = ContextDescriptorProvider.GetAll().SelectMany(c => c.DbSets.Select(p => p.PropertyType)).ToList();

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
