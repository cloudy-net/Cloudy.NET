using Cloudy.CMS.ContentTypeSupport;
using Microsoft.Extensions.Primitives;

namespace Cloudy.CMS.UI.FormSupport
{
    public interface IFormValueConverter
    {
        object Convert(string value, PropertyDefinitionDescriptor propertyDefinition);
    }
}