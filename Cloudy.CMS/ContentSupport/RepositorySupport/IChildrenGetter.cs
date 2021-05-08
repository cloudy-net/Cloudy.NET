using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IChildrenGetter
    {
        IEnumerable<T> GetChildren<T>(params object[] keyValues) where T : class;
    }
}