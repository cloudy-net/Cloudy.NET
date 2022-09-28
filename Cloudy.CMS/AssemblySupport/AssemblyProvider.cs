using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.AssemblySupport
{
    public class AssemblyProvider : IAssemblyProvider
    {
        public IEnumerable<AssemblyWrapper> Assemblies { get; }

        public AssemblyProvider(IEnumerable<AssemblyWrapper> assemblies)
        {
            Assemblies = assemblies.ToList().AsReadOnly();
        }
    }
}
