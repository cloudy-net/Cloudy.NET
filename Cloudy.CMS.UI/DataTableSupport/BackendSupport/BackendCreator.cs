using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public class BackendCreator : IBackendCreator
    {
        IComponentProvider ComponentProvider { get; }
        IInstantiator Instantiator { get; }

        public BackendCreator(IComponentProvider componentProvider, IInstantiator instantiator)
        {
            ComponentProvider = componentProvider;
            Instantiator = instantiator;
        }

        public IDictionary<string, IBackend> CreateAll()
        {
            var result = new Dictionary<string, IBackend>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
                {
                    var attribute = type.GetCustomAttribute<DataTableBackendAttribute>();

                    if (attribute == null)
                    {
                        continue;
                    }

                    if (!typeof(IBackend).IsAssignableFrom(type))
                    {
                        throw new DataTableBackendDoesNotImplementIBackendException(type);
                    }

                    result.Add(attribute.Id, (IBackend)Instantiator.Instantiate(type));
                }
            }

            return result;
        }
    }
}
