using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Methods
{
    public interface IChildrenGetter
    {
        IEnumerable<T> GetChildren<T>(params object[] keyValues) where T : class;
    }
}