using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.ScriptSupport
{
    /// <summary>
    /// Specifies a script that should be included at page load.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ScriptAttribute : Attribute
    {
        public string Path { get; }

        public ScriptAttribute(string path)
        {
            Path = path;
        }
    }
}
