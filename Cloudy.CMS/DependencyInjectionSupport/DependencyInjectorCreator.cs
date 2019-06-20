using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.DependencyInjectionSupport
{
    public class DependencyInjectorCreator : IDependencyInjectorCreator
    {
        IComponentProvider ComponentProvider { get; }
        IInstantiator Instantiator { get; }

        public DependencyInjectorCreator(IComponentProvider componentProvider, IInstantiator instantiator)
        {
            ComponentProvider = componentProvider;
            Instantiator = instantiator;
        }

        public IEnumerable<IDependencyInjector> Create()
        {
            var result = new List<IDependencyInjector>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
                {
                    if (!typeof(IDependencyInjector).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    result.Add((IDependencyInjector)Instantiator.Instantiate(type));
                }
            }

            return result;
        }
    }
}
