using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.NET.ContextSupport
{
    public class ContextDescriptorProvider : IContextDescriptorProvider
    {
        IEnumerable<ContextDescriptor> ContextDescriptors { get; }
        IDictionary<Type, ContextDescriptor> ByDbSetType { get; }

        public ContextDescriptorProvider(IEnumerable<ContextDescriptor> contextDescriptors)
        {
            ContextDescriptors = contextDescriptors.ToList().AsReadOnly();
            ByDbSetType = contextDescriptors
                .SelectMany(c => c.DbSets.Select(s => new KeyValuePair<Type, ContextDescriptor>(s.Type, c)))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public IEnumerable<ContextDescriptor> GetAll()
        {
            return ContextDescriptors;
        }

        public ContextDescriptor GetFor(Type type)
        {
            if (ByDbSetType.ContainsKey(type))
            {
                return ByDbSetType[type];
            }

            foreach (var pair in ByDbSetType)
            {
                if (pair.Key.IsAssignableFrom(type))
                {
                    return pair.Value;
                }
            }

            return null;
        }
    }
}
