using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poetry.UI.StyleSupport
{
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
