using System.Reflection;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public interface IPropertyDefinitionCreator
    {
        PropertyDefinitionDescriptor Create(PropertyInfo property);
    }
}