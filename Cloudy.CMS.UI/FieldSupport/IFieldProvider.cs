using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FieldSupport
{
    public interface IFieldProvider
    {
        IEnumerable<FieldDescriptor> Get(string entityType);
    }
}
