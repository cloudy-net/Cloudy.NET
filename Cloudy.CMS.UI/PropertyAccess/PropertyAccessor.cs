using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.PropertyAccess
{
    public class PropertyAccessor : IPropertyAccessor
    {
        public object GetProperty(Type type, object instance, string propertyName)
        {
            return TypeAccessor.Create(type)[instance, propertyName];
        }
    }
}
