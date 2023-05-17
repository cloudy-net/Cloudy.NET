using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.DependencyInjectionSupport
{
    public interface IDependencyInjector
    {
        void InjectDependencies(IServiceCollection services);
    }
}
