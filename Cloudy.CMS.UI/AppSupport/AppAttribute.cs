using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AppSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppAttribute : Attribute {
        public string Id { get; }
        public string ModulePath { get; }

        public AppAttribute(string id, string modulePath)
        {
            Id = id;
            ModulePath = modulePath;
        }
    }
}
