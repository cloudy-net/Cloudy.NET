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
        IEnumerable<BlockTypeDescriptor> BlockTypes { get; }
        Dictionary<Type, BlockTypeDescriptor> BlockTypesByType { get; }
        Dictionary<string, BlockTypeDescriptor> BlockTypesByName { get; }

        public BlockTypeProvider(IBlockTypeCreator blockTypeCreator)
        {
            BlockTypes = blockTypeCreator.Create().ToList().AsReadOnly();
            BlockTypesByType = BlockTypes.ToDictionary(t => t.Type, t => t);
            BlockTypesByName = BlockTypes.ToDictionary(t => t.Name, t => t);
        }

        public BlockTypeDescriptor Get(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return GetMostSpecificAssignableFrom(BlockTypesByType, type);
        }

        public BlockTypeDescriptor Get(string name)
        {
            if (!BlockTypesByName.ContainsKey(name))
            {
                return null;
            }

            return BlockTypesByName[name];
        }

        public IEnumerable<BlockTypeDescriptor> GetAll()
        {
            return BlockTypes;
        }

        public static BlockTypeDescriptor GetMostSpecificAssignableFrom(Dictionary<Type, BlockTypeDescriptor> types, Type type)
        {
            if (types.ContainsKey(type))
            {
                return types[type];
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
