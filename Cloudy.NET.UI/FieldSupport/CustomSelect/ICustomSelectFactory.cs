using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.FieldSupport.CustomSelect
{
    public interface ICustomSelectFactory
    {
        Task<IEnumerable<SelectListItem>> GetItems();
    }
}
