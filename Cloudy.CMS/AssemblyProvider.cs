using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS
{
    public class AssemblyProvider : IAssemblyProvider
    {
        IEnumerable<AssemblyWrapper> Assemblies { get; }

        public AssemblyProvider(IEnumerable<AssemblyWrapper> assemblies)
        {
            Assemblies = assemblies.ToList().AsReadOnly();
        }

        public IEnumerable<AssemblyWrapper> GetAll()
        {
            return Assemblies;
        }
    }
}
