using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.UI.FieldSupport
{
    public interface IFieldCreator
    {
        IEnumerable<FieldDescriptor> Create(string entityType);
    }
}