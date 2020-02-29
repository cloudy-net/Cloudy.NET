using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public interface IFieldCreator
    {
        FieldDescriptor Create(PropertyInfo property);
    }
}