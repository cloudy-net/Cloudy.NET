using Cloudy.CMS.ContentSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IRoutableRootContentProvider
    {
        IEnumerable<IContent> GetAll();
    }
}