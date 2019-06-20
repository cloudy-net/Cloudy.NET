using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace Poetry.ComponentSupport
{
    [DebuggerDisplay("{Id}")]
    public class ComponentDescriptor
    {
        public string Id { get; }
        public Type Type { get; }
        public AssemblyWrapper Assembly { get; }
        public IEnumerable<string> Dependencies { get; }

        public ComponentDescriptor(string id, Type type, AssemblyWrapper assembly, IEnumerable<string> dependencies)
        {
            Id = id;
            Type = type;
            Assembly = assembly;
            Dependencies = dependencies.ToList().AsReadOnly();
        }
    }
}
