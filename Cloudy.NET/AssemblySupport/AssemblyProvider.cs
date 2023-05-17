using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.AssemblySupport
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
