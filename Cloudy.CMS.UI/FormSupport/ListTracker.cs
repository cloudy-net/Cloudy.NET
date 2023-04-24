using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class ListTracker : IListTracker
    {
        public object Navigate(IEnumerable<object> entity, string key)
        {
            if(int.TryParse(key, out int index)) {
                return entity.ElementAt(index);
            }

            return null;
        }
    }
}
