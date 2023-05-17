using System;

namespace Cloudy.NET.DependencyInjectionSupport
{
    public interface IInstantiator
    {
        object Instantiate(Type type);
    }
}
