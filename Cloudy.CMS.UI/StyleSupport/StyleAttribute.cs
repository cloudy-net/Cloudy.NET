using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.StyleSupport
{
    /// <summary>
    /// Specifies a stylesheet that should be included at page load.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class StyleAttribute : Attribute
    {
        public string Path { get; }

        public StyleAttribute(string path)
        {
            Path = path;
        }
    }
}
