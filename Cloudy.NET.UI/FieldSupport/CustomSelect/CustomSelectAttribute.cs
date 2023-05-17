using System;

namespace Cloudy.NET.UI.FieldSupport.CustomSelect
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomSelectAttribute<T> : Attribute, ICustomSelectAttribute where T : ICustomSelectFactory
    {
    }
}
