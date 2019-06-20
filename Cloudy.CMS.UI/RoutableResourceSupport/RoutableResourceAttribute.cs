using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poetry.UI.RoutableResourceSupport
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RoutableResourceAttribute : Attribute
    {
        public string Path { get; }

        public RoutableResourceAttribute(string path)
        {
            Path = path;
        }
    }
}
