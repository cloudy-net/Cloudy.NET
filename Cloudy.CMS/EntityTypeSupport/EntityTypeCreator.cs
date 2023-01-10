using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntitySupport.Internal;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.EntityTypeSupport
{
    public record EntityTypeCreator(IContextDescriptorProvider ContextDescriptorProvider) : IEntityTypeCreator
    {
        public IEnumerable<EntityTypeDescriptor> Create()
        {
            var types = ContextDescriptorProvider.GetAll().SelectMany(c => c.DbSets.Select(p => p.Type)).ToList();

            var result = new List<EntityTypeDescriptor>();

            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                var name = type.Name;

                if (type.IsGenericType)
                {
                    name = $"{name.Split('`')[0]}<{string.Join(",", type.GetGenericArguments().Select(t => t.Name))}>";
                }

                result.Add(new EntityTypeDescriptor(
                    name,
                    type,
                    type.IsAssignableTo(typeof(INameable)),
                    type.IsAssignableTo(typeof(IImageable)),
                    type.IsAssignableTo(typeof(IRoutable)),
                    type.IsAssignableTo(typeof(ISingleton)),
                    type.IsAssignableTo(typeof(IHierarchicalMarkerInterface))
                ));
            }

            return result;
        }
    }
}
