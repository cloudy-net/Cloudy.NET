using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ListColumnAttribute : Attribute
    {
        public string UIHint { get; }

        public ListColumnAttribute() { }

        public ListColumnAttribute(string uiHint)
        {
            UIHint = uiHint;
        }

        /// <summary>
        /// Sets the display order of the column. Unordered columns start at 10000.
        /// </summary>
        public int Order { get; set; } = -10000;

        /// <summary>
        /// Sets the column name.
        /// </summary>
        public string Name { get; set; }
    }
}
