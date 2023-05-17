using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List.Filter
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ListFilterAttribute : Attribute
    {
        /// <summary>
        /// Sets the display order of the filter. Unordered filters start at 10000.
        /// </summary>
        public int Order { get; set; } = -10000;
    }
}
