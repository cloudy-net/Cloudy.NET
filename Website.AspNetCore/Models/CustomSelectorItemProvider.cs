using Cloudy.CMS.UI.FormSupport.Controls.SelectSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [ItemProvider("customselector")]
    public class CustomSelectorItemProvider : IItemProvider
    {
        Item Lorem { get; } = new Item("Lorem", null, "lorem", null, true, true);
        Item Ipsum { get; } = new Item("Ipsum", null, "ipsum", null, true, false);
        Item Dolor { get; } = new Item("Dolor", null, "dolor", null, false);
        Item Sit { get; } = new Item("Sit", null, "sit", null, false);
        Item Amet { get; } = new Item("Amet", null, "amet", null, false);

        public async Task<ItemResponse> Get(string type, string value)
        {
            if (value == "lorem")
            {
                return new ItemResponse(Lorem, Enumerable.Empty<Item>());
            }
            if (value == "ipsum")
            {
                return new ItemResponse(Ipsum, Enumerable.Empty<Item>());
            }
            if (value == "dolor")
            {
                return new ItemResponse(Dolor, Enumerable.Empty<Item>());
            }
            if (value == "sit")
            {
                return new ItemResponse(Sit, new List<Item> { Lorem });
            }
            if (value == "amet")
            {
                return new ItemResponse(Sit, new List<Item> { Ipsum });
            }

            return null;
        }

        public async Task<IEnumerable<Item>> GetAll(string type, ItemQuery query)
        {
            if (query.Parent == null)
            {
                return new List<Item> { Lorem, Ipsum, Dolor };
            }
            if (query.Parent == "lorem")
            {
                return new List<Item> { Sit };
            }
            if (query.Parent == "ipsum")
            {
                return new List<Item> { Amet };
            }

            return null;
        }
    }
}
