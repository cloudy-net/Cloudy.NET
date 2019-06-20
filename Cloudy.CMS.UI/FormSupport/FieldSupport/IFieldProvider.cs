using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.FieldSupport
{
    public interface IFieldProvider
    {
        IEnumerable<FieldDescriptor> GetAllFor(string id);
    }
}
