using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.RoutableResourceSupport
{
    public interface IRoutableResourceCreator
    {
        IEnumerable<RoutableResource> Create(ComponentDescriptor component);
    }
}
