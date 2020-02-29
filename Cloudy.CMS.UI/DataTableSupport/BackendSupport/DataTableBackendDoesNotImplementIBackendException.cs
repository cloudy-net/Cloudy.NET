using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public class DataTableBackendDoesNotImplementIBackendException : Exception
    {
        public DataTableBackendDoesNotImplementIBackendException(Type type) : base($"Class {type} was annotated with [{nameof(DataTableBackendAttribute)}] but did not implement {nameof(IBackend)}") { }
    }
}
