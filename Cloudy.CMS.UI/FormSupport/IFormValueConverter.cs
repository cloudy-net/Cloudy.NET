using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.Extensions.Primitives;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IFormValueConverter
    {
        object Convert(string value, PropertyDefinitionDescriptor propertyDefinition);
    }
}