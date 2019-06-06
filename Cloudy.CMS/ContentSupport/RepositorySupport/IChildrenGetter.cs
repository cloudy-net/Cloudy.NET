using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IChildrenGetter
    {
        IEnumerable<T> GetChildren<T>(string id, string language) where T : class;
    }
}