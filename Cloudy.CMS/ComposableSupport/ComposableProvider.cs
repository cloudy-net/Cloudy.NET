using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ComposableSupport
{
    public class ComposableProvider : IComposableProvider
    {
        IComponentProvider ComponentProvider { get; }
        IInstantiator Instantiator { get; }

        public ComposableProvider(IComponentProvider componentProvider, IInstantiator instantiator)
        {
            ComponentProvider = componentProvider;
            Instantiator = instantiator;
        }

        public IEnumerable<T> GetAll<T>() where T : IComposable
        {
            return ComponentProvider
                .GetAll()
                .Select(c => c.Assembly)
                .SelectMany(a => a.Types)
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && typeof(T).IsAssignableFrom(t))
                .Select(t => (T)Instantiator.Instantiate(t))
                .ToList()
                .AsReadOnly();
        }
    }
}
