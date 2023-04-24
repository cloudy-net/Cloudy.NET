using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IListTracker
    {
        object Navigate(IEnumerable<object> entity, string key);
    }
}