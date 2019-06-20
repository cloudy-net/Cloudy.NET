using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport
{
    public interface IComponentProvider
    {
        IEnumerable<ComponentDescriptor> GetAll();
    }
}
