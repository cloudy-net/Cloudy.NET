using Cloudy.CMS.PropertyDefinitionSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
        [Route("/{area}/api/controls/customselect/list/")]
        public async Task<IActionResult> List(
            [FromServices] IPropertyDefinitionProvider propertyDefinitionProvider,
            [FromServices] IServiceProvider serviceProvider,
            [FromQuery] string entityType,
            [FromQuery] string propertyName)
        {
            var propertyDefinition = propertyDefinitionProvider.GetFor(entityType).FirstOrDefault(x => x.Name == propertyName);

            var factoryType = propertyDefinition.Attributes
                .OfType<ICustomSelectAttribute>()
                .FirstOrDefault()
                .GetType()
                .GetGenericArguments()
                .FirstOrDefault();

            var customSelectFactory = serviceProvider.GetService(factoryType) as ICustomSelectFactory;
            var items = (await customSelectFactory.GetItems()).ToList();

            var placeholderItemText = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault()?.GetPrompt();

            return Json(new { items, placeholderItemText }, new JsonSerializerOptions().CloudyDefault());
        }
    }
}
