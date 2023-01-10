using Cloudy.CMS.EntityTypeSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.BlockSupport
{
    public interface IBlockTypeCreator
    {
        IEnumerable<BlockTypeDescriptor> Create();
    }
}