using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.Controls.SelectSupport
{
    public class ItemResponse
    {
        public Item Item { get; }
        public IEnumerable<ItemParent> Parents { get; }

        public ItemResponse(Item item, IEnumerable<ItemParent> parents)
        {
            Item = item;
            Parents = parents.ToList().AsReadOnly();
        }
    }
}