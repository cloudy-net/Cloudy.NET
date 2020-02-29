using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Cloudy.CMS.ComponentSupport;

namespace Cloudy.CMS.ComponentSupport
{
    public class ComponentCreator : IComponentCreator
    {
        ILogger<ComponentCreator> Logger { get; }
        IComponentTypeProvider ComponentTypeProvider { get; }
        IComponentAssemblyProvider ComponentAssemblyProvider { get; }
        IMissingComponentAttributeChecker MissingComponentAttributeChecker { get; }
        IMultipleComponentsInSingleAssemblyChecker MultipleComponentsInSingleAssemblyChecker { get; }
        IDuplicateComponentIdChecker DuplicateComponentIdChecker { get; }

        public ComponentCreator(ILogger<ComponentCreator> logger, IComponentTypeProvider componentTypeProvider, IComponentAssemblyProvider componentAssemblyProvider, IMissingComponentAttributeChecker missingComponentAttributeChecker, IMultipleComponentsInSingleAssemblyChecker multipleComponentsInSingleAssemblyChecker, IDuplicateComponentIdChecker duplicateComponentIdChecker)
        {
            Logger = logger;
            ComponentTypeProvider = componentTypeProvider;
            ComponentAssemblyProvider = componentAssemblyProvider;
            MissingComponentAttributeChecker = missingComponentAttributeChecker;
            MultipleComponentsInSingleAssemblyChecker = multipleComponentsInSingleAssemblyChecker;
            DuplicateComponentIdChecker = duplicateComponentIdChecker;
        }

        public IEnumerable<ComponentDescriptor> Create()
        {
            var types = ComponentTypeProvider.GetAll();

            MissingComponentAttributeChecker.Check(types);
            MultipleComponentsInSingleAssemblyChecker.Check(types);
            DuplicateComponentIdChecker.Check(types);

            var result = new List<ComponentDescriptor>();

            foreach (var type in types)
            {
                var componentId = type.GetCustomAttribute<ComponentAttribute>().Id;

                result.Add(new ComponentDescriptor(componentId, new AssemblyWrapper(type.Assembly)));
            }

            foreach(var assembly in ComponentAssemblyProvider.GetAll())
            {
                if(result.Any(c => c.Assembly.Equals(assembly))) {
                    continue;
                }

                result.Add(new ComponentDescriptor(assembly.GetName().Name, new AssemblyWrapper(assembly)));
            }

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation($"Detected {result.Count} components: {string.Join(", ", result.Select(r => r.Id))}");
            }

            return result;
        }
    }
}
