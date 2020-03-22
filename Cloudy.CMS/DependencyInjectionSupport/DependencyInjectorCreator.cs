using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public class DependencyInjectorCreator : IDependencyInjectorCreator
    {
        IComponentAssemblyProvider ComponentAssemblyProvider { get; }
        IComponentTypeProvider ComponentTypeProvider { get; }

        public DependencyInjectorCreator(IComponentAssemblyProvider componentAssemblyProvider, IComponentTypeProvider componentTypeProvider)
        {
            ComponentAssemblyProvider = componentAssemblyProvider;
            ComponentTypeProvider = componentTypeProvider;
        }

        public IEnumerable<IDependencyInjector> Create()
        {
            var result = new List<IDependencyInjector>();

            foreach (var type in ComponentAssemblyProvider.GetAll().SelectMany(a => a.GetTypes()))
            {
                if (!typeof(IDependencyInjector).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                result.Add((IDependencyInjector)Activator.CreateInstance(type));
            }

            foreach (var type in ComponentTypeProvider.GetAll().SelectMany(t => t.Assembly.GetTypes()))
            {
                if (!typeof(IDependencyInjector).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                result.Add((IDependencyInjector)Activator.CreateInstance(type));
            }

            return result;
        }
    }
}
