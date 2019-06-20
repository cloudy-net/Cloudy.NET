using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.DependencyInjectionSupport
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}
