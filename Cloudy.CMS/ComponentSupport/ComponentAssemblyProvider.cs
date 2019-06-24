using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport
{
    public class ComponentAssemblyProvider : IComponentAssemblyProvider
    {
        IEnumerable<Assembly> Assemblies { get; }

        public ComponentAssemblyProvider(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies.ToList().AsReadOnly();
        }

        public IEnumerable<Assembly> GetAll()
        {
            return Assemblies;
        }
    }
}
