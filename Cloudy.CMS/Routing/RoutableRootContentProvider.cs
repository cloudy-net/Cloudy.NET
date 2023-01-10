using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.RepositorySupport;

namespace Cloudy.CMS.Routing
{
    public class RoutableRootContentProvider : IRoutableRootContentProvider
    {
        IChildrenGetter ChildrenGetter { get; }

        public RoutableRootContentProvider(IChildrenGetter childrenGetter)
        {
            ChildrenGetter = childrenGetter;
        }

        public IEnumerable<object> GetAll()
        {
            return ChildrenGetter.GetChildren<IRoutable>(null).ToList().AsReadOnly();
        }
    }
}
