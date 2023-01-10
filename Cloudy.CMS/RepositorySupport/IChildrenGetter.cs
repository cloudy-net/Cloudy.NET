using System.Collections.Generic;

namespace Cloudy.CMS.RepositorySupport
{
    public interface IChildrenGetter
    {
        IEnumerable<T> GetChildren<T>(params object[] keyValues) where T : class;
    }
}