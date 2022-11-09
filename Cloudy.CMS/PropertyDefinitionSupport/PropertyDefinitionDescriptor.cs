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
        Func<object, object> Getter,
        Action<object, object> Setter,
        IEnumerable<object> Attributes,
        bool Nullable,
        bool List,
        bool Enum
    );
}
