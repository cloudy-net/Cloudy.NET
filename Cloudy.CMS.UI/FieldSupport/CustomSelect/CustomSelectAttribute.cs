using System;

namespace Cloudy.CMS.UI.FieldSupport.CustomSelect
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomSelectAttribute<T> : Attribute, ICustomSelectAttribute where T : ICustomSelectFactory
    {
        public bool Multi { get; set; }
    }
}
