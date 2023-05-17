using System.Reflection;

namespace Cloudy.NET.PropertyDefinitionSupport
{
    public interface IPropertyDefinitionCreator
    {
        PropertyDefinitionDescriptor Create(PropertyInfo property);
    }
}