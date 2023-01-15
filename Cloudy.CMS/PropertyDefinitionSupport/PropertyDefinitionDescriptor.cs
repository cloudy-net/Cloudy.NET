using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    [DebuggerDisplay("{Name}")]
    public record PropertyDefinitionDescriptor(
        string Name,
        Type Type,
        Func<object, object> Getter = null,
        Action<object, object> Setter = null,
        IEnumerable<Attribute> Attributes = null,
        bool Nullable = false,
        bool List = false,
        bool Enum = false,
        bool Block = false
    );
}
