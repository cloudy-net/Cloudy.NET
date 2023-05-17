using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IRoutableRootContentProvider
    {
        IEnumerable<object> GetAll();
    }
}