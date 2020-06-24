using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public class BackendCreator : IBackendCreator
    {
        IAssemblyProvider AssemblyProvider { get; }
        IInstantiator Instantiator { get; }

        public BackendCreator(IAssemblyProvider assemblyProvider, IInstantiator instantiator)
        {
            AssemblyProvider = assemblyProvider;
            Instantiator = instantiator;
        }

        public IDictionary<string, IBackend> CreateAll()
        {
            var result = new Dictionary<string, IBackend>();

            foreach (var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
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
