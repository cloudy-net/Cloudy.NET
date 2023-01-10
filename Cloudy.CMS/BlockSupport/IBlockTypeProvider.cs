using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.BlockSupport
{
    public interface IBlockTypeProvider
    {
        Type Get(Type type);
        Type Get(string name);
        IEnumerable<Type> GetAll();
    }
}
