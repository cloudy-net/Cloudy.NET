using System.Collections.Generic;

namespace Cloudy.NET.Routing
{
    public interface IRoutableRootContentProvider
    {
        IEnumerable<object> GetAll();
    }
}