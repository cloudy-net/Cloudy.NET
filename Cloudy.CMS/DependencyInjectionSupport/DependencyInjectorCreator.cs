using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.ContextSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public record DependencyInjectorCreator(IAssemblyProvider AssemblyProvider, IContextDescriptorProvider ContextDescriptorProvider) : IDependencyInjectorCreator
    {
        public IEnumerable<IDependencyInjector> Create()
        {
            var result = new List<IDependencyInjector>();

            foreach (var type in AssemblyProvider.Assemblies.SelectMany(a => a.Types))
            {
                if (!typeof(IDependencyInjector).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                {
                    var injectedAssemblyProviderConstructor = type.GetConstructor(new Type[] { typeof(IAssemblyProvider) });

                    if (injectedAssemblyProviderConstructor != null)
                    {
                        result.Add((IDependencyInjector)injectedAssemblyProviderConstructor.Invoke(new object[] { AssemblyProvider }));
                        continue;
                    }
                }

                {
                    var injectedAssemblyProviderAndContextDescriptorProviderConstructor = type.GetConstructor(new Type[] { typeof(IAssemblyProvider), typeof(IContextDescriptorProvider) });

                    if (injectedAssemblyProviderAndContextDescriptorProviderConstructor != null)
                    {
                        result.Add((IDependencyInjector)injectedAssemblyProviderAndContextDescriptorProviderConstructor.Invoke(new object[] { AssemblyProvider, ContextDescriptorProvider }));
                        continue;
                    }
                }

                result.Add((IDependencyInjector)Activator.CreateInstance(type));
            }

            return result;
        }
    }
}
