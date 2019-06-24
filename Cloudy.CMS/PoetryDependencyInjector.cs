using Poetry.ComponentSupport;
using Poetry.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Poetry.InitializerSupport;
using Poetry.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Poetry.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Poetry.ComposableSupport;
using Poetry.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Poetry.AspNetCore.DependencyInjectionSupport;

namespace Poetry
{
    public class PoetryDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<IComponentCreator, ComponentCreator>();
            container.RegisterSingleton<IComponentProvider, ComponentProvider>();
            container.RegisterSingleton<IComposableProvider, ComposableProvider>();
            container.RegisterSingleton<IMissingComponentAttributeChecker, MissingComponentAttributeChecker>();
            container.RegisterSingleton<IDuplicateComponentIdChecker, DuplicateComponentIdChecker>();
            container.RegisterSingleton<IMultipleComponentsInSingleAssemblyChecker, MultipleComponentsInSingleAssemblyChecker>();
            container.RegisterSingleton<IInitializerProvider, InitializerProvider>();
            container.RegisterSingleton<IInitializerCreator, InitializerCreator>();
            container.RegisterSingleton<IInstantiator, Instantiator>();
            container.RegisterSingleton<IDependencyInjectorCreator, DependencyInjectorCreator>();
            container.RegisterSingleton<IDependencyInjectorProvider, DependencyInjectorProvider>();
        }
    }
}
