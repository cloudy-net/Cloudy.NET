using System;

namespace Cloudy.CMS.PropertyAccess
{
    public interface IPropertySetter
    {
        void SetProperty(Type type, object instance, string propertyName, object value);
    }
}