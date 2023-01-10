using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.BlockSupport
{
    public interface IBlockTypeProvider
    {
        BlockTypeDescriptor Get(Type type);
        BlockTypeDescriptor Get(string name);
        IEnumerable<BlockTypeDescriptor> GetAll();
    }
}
