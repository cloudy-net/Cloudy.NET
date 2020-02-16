using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class CoreInterfaceProvider : ICoreInterfaceProvider
    {
        Dictionary<Type, CoreInterfaceDescriptor> CoreInterfacesByType { get; }

        public CoreInterfaceProvider(ICoreInterfaceCreator coreInterfaceCreator)
        {
            CoreInterfacesByType = coreInterfaceCreator.Create().ToDictionary(i => i.Type, i => i);
        }

        public CoreInterfaceDescriptor GetFor(Type type)
        {
            if (!CoreInterfacesByType.ContainsKey(type))
            {
                return null;
            }

            return CoreInterfacesByType[type];
        }
    }
}
