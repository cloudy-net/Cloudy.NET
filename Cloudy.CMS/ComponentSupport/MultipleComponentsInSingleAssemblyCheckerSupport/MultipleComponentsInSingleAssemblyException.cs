using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport
{
    public class MultipleComponentsInSingleAssemblyException : Exception
    {
        public MultipleComponentsInSingleAssemblyException(Assembly assembly, IEnumerable<Type> types) : base($"Assembly {assembly} has multiple components defined ({string.Join(", ", types)}). There can only be one component per assembly.") { }
    }
}
