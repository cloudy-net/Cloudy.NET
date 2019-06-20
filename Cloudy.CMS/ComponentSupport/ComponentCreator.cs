using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Poetry.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Poetry.ComponentSupport.MissingComponentDependencyCheckerSupport;
using Poetry.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Poetry.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Poetry.ComponentSupport.DependencySupport;

namespace Poetry.ComponentSupport
{
    public class ComponentCreator : IComponentCreator
    {
        ILogger<ComponentCreator> Logger { get; }
        IComponentTypeProvider ComponentTypeProvider { get; }
        IMissingComponentAttributeChecker MissingComponentAttributeChecker { get; }
        IMissingComponentDependencyChecker MissingComponentDependencyChecker { get; }
        IMultipleComponentsInSingleAssemblyChecker MultipleComponentsInSingleAssemblyChecker { get; }
        IDuplicateComponentIdChecker DuplicateComponentIdChecker { get; }
        IComponentDependencyCreator ComponentDependencyCreator { get; }

        public ComponentCreator(ILogger<ComponentCreator> logger, IComponentTypeProvider componentTypeProvider, IMissingComponentAttributeChecker missingComponentAttributeChecker, IMissingComponentDependencyChecker missingComponentDependencyChecker, IMultipleComponentsInSingleAssemblyChecker multipleComponentsInSingleAssemblyChecker, IDuplicateComponentIdChecker duplicateComponentIdChecker, IComponentDependencyCreator componentDependencyCreator)
        {
            Logger = logger;
            ComponentTypeProvider = componentTypeProvider;
            MissingComponentAttributeChecker = missingComponentAttributeChecker;
            MissingComponentDependencyChecker = missingComponentDependencyChecker;
            MultipleComponentsInSingleAssemblyChecker = multipleComponentsInSingleAssemblyChecker;
            DuplicateComponentIdChecker = duplicateComponentIdChecker;
            ComponentDependencyCreator = componentDependencyCreator;
        }

        public IEnumerable<ComponentDescriptor> Create()
        {
            var types = ComponentTypeProvider.GetAll();

            MissingComponentAttributeChecker.Check(types);
            MissingComponentDependencyChecker.Check(types);
            MultipleComponentsInSingleAssemblyChecker.Check(types);
            DuplicateComponentIdChecker.Check(types);

            var result = new List<ComponentDescriptor>();

            foreach (var type in types)
            {
                var componentId = type.GetCustomAttribute<ComponentAttribute>().Id;

                result.Add(new ComponentDescriptor(componentId, type, new AssemblyWrapper(type.Assembly), ComponentDependencyCreator.Create(type)));
            }

            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation($"Detected {result.Count} components: {string.Join(", ", result.Select(r => r.Id))}");
            }

            return result;
        }
    }
}
