using System.Collections.Generic;

namespace Cloudy.CMS.AssemblySupport
{
    public interface IAssemblyProvider
    {
        IEnumerable<AssemblyWrapper> Assemblies { get; }
    }
}