using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport.DependencySupport
{
    public interface IComponentDependencySorter
    {
        IEnumerable<ComponentDescriptor> Sort(IEnumerable<ComponentDescriptor> components);
    }
}
