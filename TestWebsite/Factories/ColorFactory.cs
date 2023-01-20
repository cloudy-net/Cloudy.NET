using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebsite.Factories
{
    public class ColorFactory : ICustomSelectFactory
    {
        public async Task<IEnumerable<SelectListItem>> GetItems()
        {
            var niceGrop = new SelectListGroup { Name = "Nice colors" };
            var darkGrop = new SelectListGroup { Name = "Dark colors" };
            var brightGroup = new SelectListGroup { Name = "Bright colors" };
            var disabledGroup = new SelectListGroup { Name = "Disabled group", Disabled = true };

            return await Task.FromResult(new[]
            {
                new SelectListItem { Text = "Red", Value = "#f56c42", Group = niceGrop },
                new SelectListItem { Text = "Blue", Value = "#02081a", Group = niceGrop },

                new SelectListItem { Text = "Black", Value = "#fff", Group = darkGrop },

                new SelectListItem { Text = "White (default item)", Value = "#000", Selected = true, Group = brightGroup },

                new SelectListItem { Text = "Red", Value = "#f56c43" },
                new SelectListItem { Text = "Blue", Value = "#02082a" },

                new SelectListItem { Text = "Red", Value = "#f56c44", Group = disabledGroup },
                new SelectListItem { Text = "Blue", Value = "#02083a", Group = disabledGroup },

                new SelectListItem { Text = "Black", Value = "#ff0", Disabled = true },
            }.OrderBy(o => o.Group?.Name).ThenBy(o => o.Text));
        }
    }
}
