using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ComposableSupport
{
    public class ComposableProvider : IComposableProvider
    {
        IAssemblyProvider AssemblyProvider { get; }
        IInstantiator Instantiator { get; }

        public ComposableProvider(IAssemblyProvider assemblyProvider, IInstantiator instantiator)
        {
            AssemblyProvider = assemblyProvider;
            Instantiator = instantiator;
        }

        public IEnumerable<T> GetAll<T>() where T : IComposable
        {
            return AssemblyProvider
                .GetAll()
                .SelectMany(a => a.Types)
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && typeof(T).IsAssignableFrom(t))
                .Select(t => (T)Instantiator.Instantiate(t))
                .ToList()
                .AsReadOnly();
        }
    }
}
