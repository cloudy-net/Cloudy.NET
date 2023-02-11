using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var selectItems = new List<object>();

            foreach (var enumValue in enumValues)
            {
                selectItems.Add(new
                {
                    text = Enum.GetName(propertyDefinition.Type, enumValue),
                    value = ((int)enumValue).ToString()
                });
            }

            return Json(selectItems, new JsonSerializerOptions().CloudyDefault());
        }
    }
}
