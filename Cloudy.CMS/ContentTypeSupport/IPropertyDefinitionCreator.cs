using System.Reflection;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface IPropertyDefinitionCreator
    {
        PropertyDefinitionDescriptor Create(PropertyInfo property);
    }
}