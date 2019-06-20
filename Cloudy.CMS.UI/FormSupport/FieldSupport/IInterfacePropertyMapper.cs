using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.UI.FormSupport.FieldSupport
{
    public interface IInterfacePropertyMapper
    {
        IEnumerable<PropertyInfo> GetFor(PropertyInfo property);
    }
}
