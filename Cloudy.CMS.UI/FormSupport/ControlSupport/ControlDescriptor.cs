using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    [DebuggerDisplay("{Id}")]
    public class ControlDescriptor
    {
        public string Id { get; }
        public string ModulePath { get; }
        public Type Type { get; }

        public ControlDescriptor(string id, string modulePath, Type type)
        {
            Id = id;
            ModulePath = modulePath;
            Type = type;
        }
    }
}
