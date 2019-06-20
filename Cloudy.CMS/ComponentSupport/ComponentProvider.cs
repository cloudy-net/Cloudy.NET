using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.ComponentSupport
{
    public class ComponentProvider : IComponentProvider
    {
        IEnumerable<ComponentDescriptor> Components { get; }

        public ComponentProvider(IComponentCreator componentCreator)
        {
            Components = componentCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<ComponentDescriptor> GetAll()
        {
            return Components;
        }
    }
}
