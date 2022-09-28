using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DataMethods
{
    public class ChildrenGetter : IChildrenGetter
    {
        public IEnumerable<T> GetChildren<T>(params object[] keyValues) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
