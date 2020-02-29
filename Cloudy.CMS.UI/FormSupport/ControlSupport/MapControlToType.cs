using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MapControlToTypeAttribute : Attribute
    {
        public Type Type { get; }

        public MapControlToTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
