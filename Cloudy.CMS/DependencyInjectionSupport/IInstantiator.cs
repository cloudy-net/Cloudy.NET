using System;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public interface IInstantiator
    {
        object Instantiate(Type type);
    }
}
