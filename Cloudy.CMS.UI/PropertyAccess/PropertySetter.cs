using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.PropertyAccess
{
    public class PropertySetter : IPropertySetter
    {
        public void SetProperty(Type type, object instance, string propertyName, object value)
        {
            TypeAccessor.Create(type)[instance, propertyName] = value;
        }
    }
}
