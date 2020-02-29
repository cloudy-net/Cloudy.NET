using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class PropertyAttributeInheritor : IPropertyAttributeInheritor
    {
        IInterfacePropertyMapper InterfacePropertyMapper { get; }

        public PropertyAttributeInheritor(IInterfacePropertyMapper interfacePropertyMapper)
        {
            InterfacePropertyMapper = interfacePropertyMapper;
        }

        public IEnumerable<T> GetFor<T>(PropertyInfo property) where T : Attribute
        {
            var properties = new List<PropertyInfo>();

            properties.Add(property);
            properties.AddRange(InterfacePropertyMapper.GetFor(property));

            foreach(var p in properties)
            {
                var attributes = p.GetCustomAttributes<T>();

                if (!attributes.Any())
                {
                    continue;
                }

                return attributes;
            }

            return Enumerable.Empty<T>();
        }
    }
}
