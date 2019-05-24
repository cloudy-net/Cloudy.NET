using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    [DebuggerDisplay("{Name}")]
    public class PropertyDefinitionDescriptor
    {
        public string Name { get; }
        public Type Type { get; }
        public Func<object, object> Getter { get; }
        public Action<object, object> Setter { get; }
        public IEnumerable<object> Attributes { get; }

        public PropertyDefinitionDescriptor(string name, Type type, Func<object, object> getter, Action<object, object> setter, IEnumerable<object> attributes)
        {
            Name = name;
            Type = type;
            Getter = getter;
            Setter = setter;
            Attributes = attributes.ToList().AsReadOnly();
        }
    }
}
