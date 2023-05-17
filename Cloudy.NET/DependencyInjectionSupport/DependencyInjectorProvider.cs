using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public class DependencyInjectorProvider : IDependencyInjectorProvider
    {
        IEnumerable<IDependencyInjector> DependencyInjectors { get; }

        public DependencyInjectorProvider(IDependencyInjectorCreator dependencyInjectorCreator)
        {
            DependencyInjectors = dependencyInjectorCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<IDependencyInjector> GetAll()
        {
            return DependencyInjectors;
        }
    }
}
