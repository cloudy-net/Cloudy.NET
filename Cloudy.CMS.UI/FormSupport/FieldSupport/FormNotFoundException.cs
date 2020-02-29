using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class FormNotFoundException : Exception
    {
        public FormNotFoundException(string id) : base($"Form with ID {id} was not found") { }
    }
}
