using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class CoreInterfaceAttribute : Attribute
    {
        public string Id { get; }

        public CoreInterfaceAttribute(string id)
        {
            Id = id;
        }
    }
}
