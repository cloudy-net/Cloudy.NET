using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ContentRouteAttribute : Attribute
    {
        public Type Type { get; }

        public ContentRouteAttribute(Type type)
        {
            Type = type;
        }
    }
}
