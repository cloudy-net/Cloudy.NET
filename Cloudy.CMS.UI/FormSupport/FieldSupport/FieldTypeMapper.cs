using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using System;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class FieldTypeMapper : IFieldTypeMapper
    {
        public string MapToPartial(PropertyDefinitionDescriptor propertyDefinitionDescriptor)
        {
            if(propertyDefinitionDescriptor.Attributes.Any(a => a is SelectAttribute))
            {
                return "select";
            }

            if(propertyDefinitionDescriptor.Type == typeof(string))
            {
                return "text";
            }

            return "failed";
        }
    }
}