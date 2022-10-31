using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
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
        public void Update(ContentTypeDescriptor contentType, IEnumerable<string> primaryKeyNames, object instance, IFormCollection form)
        {
            var propertyDefinitions = PropertyDefinitionProvider.GetFor(contentType.Name).ToDictionary(p => p.Name, p => p);

            foreach (var field in FieldProvider.Get(contentType.Name))
            {
                if (!propertyDefinitions.ContainsKey(field.Name))
                {
                    continue;
                }

                if (primaryKeyNames.Contains(field.Name))
                {
                    continue;
                }

                var propertyDefinition = propertyDefinitions[field.Name];

                var formValue = form[field.Name].FirstOrDefault();
                object value;

                try
                {
                    value = FormValueConverter.Convert(formValue, propertyDefinition);
                }
                catch (Exception exception)
                {
                    throw new Exception($"Could not convert value \"{formValue}\" to {propertyDefinition.Type} on {contentType.Name} property {propertyDefinition.Name}", exception);
                }

                propertyDefinition.Setter(instance, value);
            }
        }
    }
}
