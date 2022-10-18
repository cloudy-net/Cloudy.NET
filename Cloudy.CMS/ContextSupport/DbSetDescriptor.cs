using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContextSupport
{
    public record DbSetDescriptor(
        Type Type,
        PropertyInfo PropertyInfo
    );
}
