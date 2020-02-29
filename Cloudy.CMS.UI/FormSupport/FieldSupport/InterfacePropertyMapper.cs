using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class InterfacePropertyMapper : IInterfacePropertyMapper
    {
        public IEnumerable<PropertyInfo> GetFor(PropertyInfo property)
        {
            var type = property.DeclaringType;
            var result = new List<PropertyInfo>();

            foreach (var @interface in type.GetInterfaces())
            {
                var interfaceProperty = @interface.GetProperty(property.Name);

                if (interfaceProperty == null)
                {
                    continue;
                }

                var getter = interfaceProperty.GetGetMethod();
                var setter = interfaceProperty.GetSetMethod();

                if (getter == null || setter == null)
                {
                    continue;
                }

                result.Add(interfaceProperty);
            }

            return result;
        }
    }
}
