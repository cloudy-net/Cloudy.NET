using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport.Select
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class EnumSelectController : Controller
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IEntityTypeProvider EntityTypeProvider { get; }

        public EnumSelectController(IPropertyDefinitionProvider propertyDefinitionProvider, IEntityTypeProvider entityTypeProvider)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            EntityTypeProvider = entityTypeProvider;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/select/enum")]
        public async Task<IActionResult> List([FromQuery] string entityType, [FromQuery] string propertyName)
        {
            var type = EntityTypeProvider.Get(entityType);
            var propertyDefinition = PropertyDefinitionProvider.GetFor(type.Name)
                .FirstOrDefault(x => x.Name == propertyName);
            
            var enumValues = Enum.GetValues(propertyDefinition.Type);
            var selectItems = new List<SelectItem>();

            foreach (var enumValue in enumValues)
            {
                var name = Enum.GetName(propertyDefinition.Type, enumValue);
                var displayAttribute = propertyDefinition.Type.GetMember(name)
                    .FirstOrDefault()
                    .GetCustomAttributes(false)
                    .OfType<DisplayAttribute>()
                    .FirstOrDefault();

                var displayName = displayAttribute?.GetName() ?? name;
                var order = displayAttribute?.GetOrder() ?? int.MaxValue;

                selectItems.Add(new SelectItem
                {
                    Order = order,
                    Text = displayName,
                    Value = ((int)enumValue).ToString()
                });
            }

            return Json(selectItems.OrderBy(x => x.Order).ThenBy(x => x.Text).ToArray(), new JsonSerializerOptions().CloudyDefault());
        }
    }

    internal class SelectItem
    { 
        public int Order { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
