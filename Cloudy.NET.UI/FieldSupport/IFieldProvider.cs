using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.UI.FieldSupport
{
    public interface IFieldProvider
    {
        IEnumerable<FieldDescriptor> Get(string entityType);
    }
}
