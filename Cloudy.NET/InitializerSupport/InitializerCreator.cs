using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.InitializerSupport
{
    public class InitializerCreator : IInitializerCreator
    {
        IAssemblyProvider AssemblyProvider { get; }
        IInstantiator Instantiator { get; }

        public InitializerCreator(IAssemblyProvider assemblyProvider, IInstantiator instantiator)
        {
            AssemblyProvider = assemblyProvider;
            Instantiator = instantiator;
        }

        public IEnumerable<IInitializer> Create()
        {
            var result = new List<IInitializer>();

            foreach (var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
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
