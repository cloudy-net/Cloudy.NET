using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.DependencyInjectionSupport
{
    public interface IDependencyInjectorCreator
    {
        IEnumerable<IDependencyInjector> Create();
    }
}
