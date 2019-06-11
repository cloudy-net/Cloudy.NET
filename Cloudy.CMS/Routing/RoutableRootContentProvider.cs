using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;

namespace Cloudy.CMS.Routing
{
    public class RoutableRootContentProvider : IRoutableRootContentProvider
    {
        IChildrenGetter ChildrenGetter { get; }

        public RoutableRootContentProvider(IChildrenGetter childrenGetter)
        {
            ChildrenGetter = childrenGetter;
        }

        public IEnumerable<IContent> GetAll()
        {
            return ChildrenGetter.GetChildren<IRoutable>(null, null).Cast<IContent>().ToList().AsReadOnly();
        }
    }
}
