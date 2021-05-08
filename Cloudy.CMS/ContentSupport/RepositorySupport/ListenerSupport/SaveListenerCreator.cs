using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public class SaveListenerCreator : ISaveListenerCreator
    {
        ILogger Logger { get; }
        IAssemblyProvider AssemblyProvider { get; }
        IInstantiator Instantiator { get; }

        public SaveListenerCreator(ILogger<SaveListenerCreator> logger, IAssemblyProvider assemblyProvider, IInstantiator instantiator)
        {
            Logger = logger;
            AssemblyProvider = assemblyProvider;
            Instantiator = instantiator;
        }

        public IEnumerable<ISaveListener<object>> Create()
        {
            var result = new List<ISaveListener<object>>();

            foreach(var type in AssemblyProvider.GetAll().SelectMany(a => a.Types))
            {
                if (!type.IsClass)
                {
                    continue;
                }

                if (type.IsAbstract)
                {
                    continue;
                }

                if (!typeof(ISaveListener<object>).IsAssignableFrom(type))
                {
                    continue;
                }

                Logger.LogInformation($"Creating SaveListener {type}");

                result.Add((ISaveListener<object>)Instantiator.Instantiate(type));
            }

            return result.AsReadOnly();
        }
    }
}
