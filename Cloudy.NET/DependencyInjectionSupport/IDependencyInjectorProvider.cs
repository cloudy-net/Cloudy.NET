using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.DependencyInjectionSupport
{
    public interface IDependencyInjectorProvider
    {
        IEnumerable<IDependencyInjector> GetAll();
    }
}
