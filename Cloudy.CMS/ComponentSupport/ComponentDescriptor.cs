using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace Cloudy.CMS.ComponentSupport
{
    [DebuggerDisplay("{Id}")]
    public class ComponentDescriptor
    {
        public string Id { get; }
        public AssemblyWrapper Assembly { get; }

        public ComponentDescriptor(string id, AssemblyWrapper assembly)
        {
            Id = id;
            Assembly = assembly;
        }
    }
}
