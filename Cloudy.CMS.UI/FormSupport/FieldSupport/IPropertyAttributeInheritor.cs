using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public interface IPropertyAttributeInheritor
    {
        IEnumerable<T> GetFor<T>(PropertyInfo property) where T : Attribute;
    }
}