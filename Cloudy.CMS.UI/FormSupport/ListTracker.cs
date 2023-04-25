using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cloudy.CMS.UI.FormSupport
{
    public class ListTracker : IListTracker
    {
        IDictionary<Tuple<object, string>, object> ListElements { get; } = new Dictionary<Tuple<object, string>, object>();

        public object GetElement(object list, string key)
        {
            if (ListElements.TryGetValue(Tuple.Create(list, key), out var element))
            {
                return element;
            }

            if (int.TryParse(key, out int index))
            {
                return ((IEnumerable<object>)list).ElementAt(index);
            }

            return null;
        }

        public void AddElement(object list, string key, object element)
        {
            ListElements[Tuple.Create(list, key)] = element;
        }
    }
}
