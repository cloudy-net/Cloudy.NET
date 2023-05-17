using Cloudy.NET.UI.FieldSupport.CustomSelect;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebsite.Factories
{
    public interface IColorFactory : ICustomSelectFactory { }

    public class ColorFactory : IColorFactory
    {
        public async Task<IEnumerable<SelectListItem>> GetItems()
        {
            var brightGroup = new SelectListGroup { Name = "Bright colors" };
            var coolGroup = new SelectListGroup { Name = "Cool colors", Disabled = true };

            return await Task.FromResult(new[]
            {
                new SelectListItem { Text = "Black", Value = "#fff" },
                new SelectListItem { Text = "Blue", Value = "#02081a" },
                new SelectListItem { Text = "Red", Value = "#f56c43" },

                new SelectListItem { Text = "White (default item)", Value = "#000", Group = brightGroup },

                new SelectListItem { Text = "Red", Value = "#f56c44", Disabled = true, Group = coolGroup },
                new SelectListItem { Text = "Blue", Value = "#02083a", Disabled = true, Group = coolGroup },
                new SelectListItem { Text = "Black 2nd", Value = "#ff0", Group = coolGroup  },

            }.OrderBy(o => o.Group?.Name).ThenBy(o => o.Text));
        }
    }
}
