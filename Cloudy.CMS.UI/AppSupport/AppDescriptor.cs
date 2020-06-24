using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppDescriptor
    {
        public string Id { get; }
        public string ModulePath { get; }
        public string Name { get; }

        public AppDescriptor(string id, string modulePath, string name)
        {
            Id = id;
            ModulePath = modulePath;
            Name = name;
        }
    }
}
