using System;
using System.Collections.Generic;

namespace Poetry.ComponentSupport
{
    public interface IComponentCreator
    {
        IEnumerable<ComponentDescriptor> Create();
    }
}