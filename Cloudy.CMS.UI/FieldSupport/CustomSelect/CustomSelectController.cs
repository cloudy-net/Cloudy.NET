using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport.CustomSelect
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class CustomSelectController : Controller
    {
        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/customselect/list/{factoryAssemblyQualifiedName}/")]
        public async Task<IActionResult> List([FromServices] IServiceProvider serviceProvider, string factoryAssemblyQualifiedName)
        {
            var factoryType = Type.GetType(factoryAssemblyQualifiedName);
            var customSelectFactory = serviceProvider.GetService(factoryType) as ICustomSelectFactory;
            var items = await customSelectFactory.GetItems();

            return Json(items.ToList(), new JsonSerializerOptions().CloudyDefault());
        }
    }
}
