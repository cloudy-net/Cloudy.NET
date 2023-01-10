using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.BlockSupport
{
    public class BlockTypeProvider : IBlockTypeProvider
    {
        IEnumerable<Type> Types { get; }
        Dictionary<string, Type> TypesByName { get; }

        public BlockTypeProvider(IBlockTypeCreator blockTypeCreator)
        {
            Types = blockTypeCreator.Create().ToList().AsReadOnly();
            TypesByName = Types.ToDictionary(t => t.Name, t => t);
        }

        public Type Get(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(Types, type);
        }

        public Type Get(string name)
        {
            if (!TypesByName.ContainsKey(name))
            {
                return null;
            }

            return TypesByName[name];
        }

        public IEnumerable<Type> GetAll()
        {
            return Types;
        }

        public static Type GetMostSpecificAssignableFrom(IEnumerable<Type> types, Type type)
        {
            if (types.Contains(type))
            {
                return type;
            }

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(types, baseType);
        }
    }
}
