using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS
{
    [DebuggerDisplay("{Assembly != null ? Assembly.GetName().Name : \"{\" + string.Join(\", \", Types) + \"}\"}")]
    public class AssemblyWrapper
    {
        public Assembly Assembly { get; }
        public IEnumerable<Type> Types { get; }

        public AssemblyWrapper(Assembly assembly)
        {
            Assembly = assembly;
            Types = assembly.ExportedTypes.ToList().AsReadOnly();
        }

        public AssemblyWrapper(IEnumerable<Type> types)
        {
            Types = types.ToList().AsReadOnly();
        }

        public override bool Equals(object obj)
        {
            if(Assembly == null)
            {
                if(obj == null)
                {
                    return false;
                }

                return obj.Equals(this);
            }

            return Assembly.Equals((obj as AssemblyWrapper)?.Assembly ?? obj);
        }

        public override int GetHashCode()
        {
            if(Assembly == null)
            {
                return 0;
            }

            return Assembly.GetHashCode();
        }
    }
}
