using System;
using System.Collections.Generic;
using System.Reflection;

namespace Poetry.UI.FormSupport.FieldSupport
{
    public interface IFieldCreator
    {
        FieldDescriptor Create(PropertyInfo property);
    }
}