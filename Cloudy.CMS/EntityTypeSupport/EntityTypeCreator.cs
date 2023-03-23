using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.EntitySupport.HierarchySupport.Internal;
using System.Runtime.CompilerServices;

namespace Cloudy.CMS.EntityTypeSupport
{
    public record EntityTypeCreator(IContextDescriptorProvider ContextDescriptorProvider, IAssemblyProvider AssemblyProvider) : IEntityTypeCreator
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
                    true,
                    type.IsAssignableTo(typeof(INameable)),
                    type.IsAssignableTo(typeof(IImageable)),
                    type.IsAssignableTo(typeof(IRoutable)),
                    type.IsAssignableTo(typeof(ISingleton)),
                    type.IsAssignableTo(typeof(IHierarchicalMarkerInterface))
                ));
            }

            var propertyReferencedBlockTypes = result.SelectMany(t => t.Type.GetProperties())
                .Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null)
                .Select(p => p.PropertyType)
                .Select(propertyType => propertyType.IsGenericType && propertyType  .GetGenericTypeDefinition() == typeof(IList<>) ? propertyType.GetGenericArguments().Single() : propertyType)
                .Where(propertyType => propertyType != typeof(string) && !propertyType.IsAssignableTo(typeof(ITuple)) && (propertyType.IsClass || propertyType.IsInterface))
                .ToList().AsReadOnly();

            var blockTypes = AssemblyProvider.GetAll()
                .SelectMany(a => a.Types)
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => propertyReferencedBlockTypes.Any(b => t.IsAssignableTo(b)))
                .Select(t => new EntityTypeDescriptor(t.Name, t));

            result.AddRange(blockTypes);

            return result;
        }
    }
}
