using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.DataTableSupport.BackendSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTableBackendAttribute : Attribute
    {
        public string Id { get; }

        public DataTableBackendAttribute(string id)
        {
            Id = id;
        }
    }
}
