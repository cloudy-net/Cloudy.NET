using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.DependencyInjectionSupport
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}
