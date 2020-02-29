using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.ComponentSupport;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppDescriptor
    {
        public string Id { get; }
        public string ComponentId { get; }
        public string ModulePath { get; }
        public string Name { get; }

        public AppDescriptor(string id, string componentId, string modulePath, string name)
        {
            Id = id;
            ComponentId = componentId;
            ModulePath = modulePath;
            Name = name;
        }
    }
}
