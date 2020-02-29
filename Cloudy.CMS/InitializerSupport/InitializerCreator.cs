using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.InitializerSupport
{
    public class InitializerCreator : IInitializerCreator
    {
        IComponentProvider ComponentProvider { get; }
        IInstantiator Instantiator { get; }

        public InitializerCreator(IComponentProvider componentProvider, IInstantiator instantiator)
        {
            ComponentProvider = componentProvider;
            Instantiator = instantiator;
        }

        public IEnumerable<IInitializer> Create()
        {
            var result = new List<IInitializer>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
                {
                    if (!typeof(IInitializer).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    if (type.IsAbstract || type.IsInterface)
                    {
                        continue;
                    }

                    result.Add((IInitializer)Instantiator.Instantiate(type));
                }
            }

            return result;
        }
    }
}
