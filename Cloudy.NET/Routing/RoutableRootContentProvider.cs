using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.NET.EntitySupport;
using Cloudy.NET.RepositorySupport;

namespace Cloudy.NET.Routing
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
