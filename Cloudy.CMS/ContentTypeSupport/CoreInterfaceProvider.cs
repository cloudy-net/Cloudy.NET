using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class CoreInterfaceProvider : ICoreInterfaceProvider
    {
        Dictionary<Type, CoreInterfaceDescriptor> InterfacesByType { get; }

        public CoreInterfaceProvider(ICoreInterfaceCreator coreInterfaceCreator)
        {
            InterfacesByType = coreInterfaceCreator.Create().ToDictionary(i => i.Type, i => i);
        }

        public CoreInterfaceDescriptor GetFor(Type type)
        {
            if (!InterfacesByType.ContainsKey(type))
            {
                return null;
            }

            return InterfacesByType[type];
        }
    }
}
