using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
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
            if (ContextDescriptorsByDbSetType.ContainsKey(type))
            {
                return ContextDescriptorsByDbSetType[type];
            }

            foreach(var pair in ContextDescriptorsByDbSetType)
            {
                if (pair.Key.IsAssignableFrom(type))
                {
                    return pair.Value;
                }
            }

            throw new CouldNotFindAnyDbContextWithDbSetForTypeException(type);
        }
    }
}
