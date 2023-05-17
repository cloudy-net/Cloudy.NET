using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public interface IDependencyInjectorCreator
    {
        IEnumerable<IDependencyInjector> Create();
    }
}
