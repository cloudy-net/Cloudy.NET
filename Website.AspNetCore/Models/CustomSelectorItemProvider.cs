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
        public async Task<Item> Get(string type, string value)
        {
            return new List<Item>
            {
                    new Item("Lorem", null, "lorem", null, true, true),
                    new Item("Ipsum", null, "ipsum", null, true, false),
                    new Item("Dolor", null, "dolor", null, false),
                    new Item("Sit amet", null, "sit-amet", null, false),
                    new Item("Vestibulum", null, "vestibulum", null, false),
            }.SingleOrDefault(i => i.Value == value);
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
