using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;

namespace Poetry.EmbeddedResourceSupport
{
    public class EmbeddedResourceCreator : IEmbeddedResourceCreator
    {
        public IEnumerable<EmbeddedResource> Create(ComponentDescriptor component)
        {
            var assemblyName = component.Assembly.Assembly.GetName().Name;
            return component.Assembly.Assembly.GetManifestResourceNames().Select(resourceName => new EmbeddedResource(assemblyName, resourceName));
        }
    }
}
