using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public class BackendNotFoundException : Exception
    {
        public BackendNotFoundException(string id) : base($"Data table backend with ID {id} was not found") { }
    }
}
