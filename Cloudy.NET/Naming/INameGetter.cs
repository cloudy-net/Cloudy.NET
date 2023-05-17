using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.Naming
{
    public interface INameGetter
    {
        string GetName(object instance);
    }
}
