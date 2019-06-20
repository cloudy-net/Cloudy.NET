using Poetry.ComponentSupport;
using Poetry.EmbeddedResourceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Poetry.UI.RoutableResourceSupport
{
    public class RoutableResourceCreator : IRoutableResourceCreator
    {
        public IEnumerable<RoutableResource> Create(ComponentDescriptor component)
        {
            var result = new List<RoutableResource>();

            foreach (var attribute in component.Type.GetCustomAttributes<RoutableResourceAttribute>())
            {
                if (attribute.Path.StartsWith("/"))
                {
                    throw new AbsoluteRoutableResourcePathException(component.Type, attribute.Path);
                }

                result.Add(new RoutableResource(attribute.Path));
            }

            return result.AsReadOnly();
        }
    }
}
