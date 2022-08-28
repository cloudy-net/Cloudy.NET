using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class PropertyDefinitionCreator : IPropertyDefinitionCreator
    {
        public PropertyDefinitionDescriptor Create(PropertyInfo property)
        {
            return new PropertyDefinitionDescriptor(property.Name, property.PropertyType, (instance) => property.GetValue(instance), (instance, value) => property.SetValue(instance, value), property.GetCustomAttributes());
        }
    }
}
