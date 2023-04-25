using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IListTracker
    {
        object GetElement(object list, string key);
        void AddElement(object list, string key, object element);
    }
}