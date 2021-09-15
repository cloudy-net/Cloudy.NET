using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public class ContextDescriptorProvider : IContextDescriptorProvider
    {
        IDictionary<Type, ContextDescriptor> ContextDescriptorsByDbSetType { get; }

        public ContextDescriptorProvider(IEnumerable<ContextDescriptor> contextDescriptors)
        {
            ContextDescriptorsByDbSetType = contextDescriptors
                .SelectMany(c => c.DbSets.Select(s => new KeyValuePair<Type, ContextDescriptor>(s.PropertyType.GetGenericArguments()[0], c)))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public ContextDescriptor GetFor(Type type)
        {
            return ContextDescriptorsByDbSetType[type];
        }
    }
}
