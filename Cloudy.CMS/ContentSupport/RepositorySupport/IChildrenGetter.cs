using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IChildrenGetter
    {
        IEnumerable<T> GetChildren<T>(string id) where T : class;
    }
}