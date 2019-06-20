using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;

namespace Poetry.UI.RoutableResourceSupport
{
    public class RoutableResourceProvider : IRoutableResourceProvider
    {
        IDictionary<string, IEnumerable<RoutableResource>> RoutableResources { get; }

        public RoutableResourceProvider(IRoutableResourceCreator routableResourceCreator, IComponentProvider componentProvider)
        {
            RoutableResources = componentProvider.GetAll().ToDictionary(c => c.Id, c => (IEnumerable<RoutableResource>)routableResourceCreator.Create(c).ToList().AsReadOnly());
        }

        public IEnumerable<RoutableResource> GetAllFor(ComponentDescriptor component)
        {
            return RoutableResources[component.Id];
        }
    }
}
