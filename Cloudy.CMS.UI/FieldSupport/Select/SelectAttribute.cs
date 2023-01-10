using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport.Select
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectAttribute : Attribute
    {
        public Type Type { get; }

        public SelectAttribute(Type type)
        {
            Type = type;
        }
    }
}
