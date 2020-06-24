using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.PropertyAccess
{
    public interface IPropertyGetter
    {
        object GetProperty(Type type, object instance, string propertyName);
    }
}
