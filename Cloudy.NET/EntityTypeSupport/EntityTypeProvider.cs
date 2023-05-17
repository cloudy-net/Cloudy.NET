using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.EntityTypeSupport
{
    public class EntityTypeProvider : IEntityTypeProvider
    {
        IEnumerable<EntityTypeDescriptor> EntityTypes { get; }
        Dictionary<Type, EntityTypeDescriptor> EntityTypesByType { get; }
        Dictionary<string, EntityTypeDescriptor> EntityTypesByName { get; }

        public EntityTypeProvider(IEntityTypeCreator entityTypeCreator)
        {
            EntityTypes = entityTypeCreator.Create().ToList().AsReadOnly();
            EntityTypesByType = EntityTypes.ToDictionary(t => t.Type, t => t);
            EntityTypesByName = EntityTypes.ToDictionary(t => t.Name, t => t);
        }

        public EntityTypeDescriptor Get(Type type)
        {
            if(type == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(EntityTypesByType, type);
        }

        public EntityTypeDescriptor Get(string name)
        {
            if (!EntityTypesByName.ContainsKey(name))
            {
                return null;
            }

            return EntityTypesByName[name];
        }

        public IEnumerable<EntityTypeDescriptor> GetAll()
        {
            return EntityTypes;
        }

        public static EntityTypeDescriptor GetMostSpecificAssignableFrom(Dictionary<Type, EntityTypeDescriptor> entityTypes, Type type)
        {
            if (entityTypes.ContainsKey(type))
            {
                return entityTypes[type];
            }

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(entityTypes, baseType);
        }
    }
}
