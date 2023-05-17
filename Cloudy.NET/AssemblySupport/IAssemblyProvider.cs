using System.Collections.Generic;

namespace Cloudy.NET.AssemblySupport
{
    public interface IAssemblyProvider
    {
        IEnumerable<AssemblyWrapper> GetAll();
    }
}