using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public interface IFieldProvider
    {
        IEnumerable<FieldDescriptor> GetAllFor(string id);
    }
}
