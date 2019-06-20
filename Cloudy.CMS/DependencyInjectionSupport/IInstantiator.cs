using System;

namespace Poetry.DependencyInjectionSupport
{
    public interface IInstantiator
    {
        object Instantiate(Type type);
    }
}
