using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public class ControlAttribute : Attribute
    {
        public string Id { get; }
        public string ModulePath { get; }

        public ControlAttribute(string id, string modulePath)
        {
            Id = id;
            ModulePath = modulePath;
        }
    }
}
