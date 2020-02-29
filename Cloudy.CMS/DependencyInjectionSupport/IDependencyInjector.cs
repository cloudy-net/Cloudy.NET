using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public interface IDependencyInjector
    {
        void InjectDependencies(IContainer container);
    }
}
