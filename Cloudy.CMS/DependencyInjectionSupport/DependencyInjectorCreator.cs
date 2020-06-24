using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public class DependencyInjectorCreator : IDependencyInjectorCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public DependencyInjectorCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<IDependencyInjector> Create()
        {
            var result = new List<IDependencyInjector>();

            foreach (var type in AssemblyProvider.GetAll().SelectMany(a => a.Types))
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
