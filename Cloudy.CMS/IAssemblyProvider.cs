using System.Collections.Generic;

namespace Cloudy.CMS
{
    public interface IAssemblyProvider
    {
        IEnumerable<AssemblyWrapper> GetAll();
    }
}