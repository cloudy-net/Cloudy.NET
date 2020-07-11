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
        public Task<Item> Get(string type, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Item>> GetAll(string type, ItemQuery query)
        {
            if (query.Parent == null)
            {
                return new List<Item> {
                    new Item("Lorem", null, "lorem", null, true, true),
                    new Item("Ipsum", null, "ipsum", null, true, false),
                    new Item("Dolor", null, "dolor", null, false),
                };
            }
            if (query.Parent == "lorem")
            {
                return new List<Item> {
                    new Item("Sit amet", null, "sit-amet", null, false),
                };
            }
            if (query.Parent == "ipsum")
            {
                return new List<Item> {
                    new Item("Vestibulum", null, "vestibulum", null, false),
                };
            }

            return null;
        }
    }
}
