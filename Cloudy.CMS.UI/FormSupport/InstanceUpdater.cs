using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public record InstanceUpdater(IFieldProvider FieldProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IFormValueConverter FormValueConverter) : IInstanceUpdater
    {
        public void Update(string contentType, object instance, IFormCollection form)
        {
            var propertyDefinitions = PropertyDefinitionProvider.GetFor(contentType).ToDictionary(p => p.Name, p => p);

            foreach(var field in FieldProvider.Get(contentType))
            {
                if (!propertyDefinitions.ContainsKey(field.Name))
                {
                    continue;
                }

                var propertyDefinition = propertyDefinitions[field.Name];

                var value = FormValueConverter.Convert(form[field.Name].FirstOrDefault(), propertyDefinition);

                if(propertyDefinition.Type == typeof(string) && (value as string) == "")
                {
                    value = null;
                }

                propertyDefinition.Setter(instance, value);
            }
        }
    }
}
