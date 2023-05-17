using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.RepositorySupport
{
    public class ChildrenGetter : IChildrenGetter
    {
        public IEnumerable<T> GetChildren<T>(params object[] keyValues) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
