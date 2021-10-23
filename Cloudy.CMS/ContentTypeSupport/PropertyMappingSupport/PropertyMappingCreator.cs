using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport
{
    public class PropertyMappingCreator : IPropertyMappingCreator
    {
        public PropertyMapping Create(PropertyInfo property)
        {
            if (property.GetCustomAttribute<CloudyIgnore>() != null)
            {
                return new PropertyMapping(PropertyMappingType.Ignored);
            }

            foreach (var interfaceType in property.DeclaringType.GetInterfaces())
            {
                var interfaceProperty = interfaceType.GetProperty(property.Name);

                if(interfaceProperty == null)
                {
                    continue;
                }

                var getter = interfaceProperty.GetGetMethod();
                var setter = interfaceProperty.GetSetMethod();

                if (getter == null || setter == null)
                {
                    continue;
                }

                var interfaceMap = property.DeclaringType.GetInterfaceMap(interfaceType);

                var explicitGetterName = $"{interfaceType.FullName.Replace('+', '.')}.{getter.Name}";
                var explicitSetterName = $"{interfaceType.FullName.Replace('+', '.')}.{setter.Name}";

                var isExplicit = interfaceMap.TargetMethods.Any(m => m.Name == explicitGetterName) && interfaceMap.TargetMethods.Any(m => m.Name == explicitSetterName);

                if (!isExplicit)
                {
                    return new PropertyMapping(PropertyMappingType.CoreInterface);
                }
            }

            if(property.GetGetMethod() == null || property.GetSetMethod() == null)
            {
                return new PropertyMapping(PropertyMappingType.Incomplete);
            }

            return new PropertyMapping(PropertyMappingType.Own);
        }
    }
}
