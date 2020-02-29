using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport
{
    public class DuplicateComponentIdException : Exception
    {
        public DuplicateComponentIdException(string id, IEnumerable<Assembly> assemblies) : base($"The same component id ({id}) was used by different assemblies ({string.Join(" and ", assemblies)})") { }
    }
}
