using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class ListTracker : IListTracker
    {
        IDictionary<Tuple<object, string>, object> Elements { get; } = new Dictionary<Tuple<object, string>, object>();

        public object GetElement(IEnumerable<object> list, string key)
        {
            if(int.TryParse(key, out int index)) {
                return list.ElementAt(index);
            }

            return null;
        }
        public void AddElement(object list, string key, object element)
        {
            throw new NotImplementedException();
        }
    }
}
