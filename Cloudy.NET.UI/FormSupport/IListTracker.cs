using System.Collections.Generic;

namespace Cloudy.NET.UI.FormSupport
{
    public interface IListTracker
    {
        object GetElement(object list, string key);
        void AddElement(object list, string key, object element);
        void RemoveElement(object list, string key);
    }
}