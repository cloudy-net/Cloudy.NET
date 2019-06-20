using Microsoft.Extensions.Logging;
using Poetry.ComponentSupport;
using Poetry.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry
{
    public class PoetryConfigurator
    {
        public IContainer Container { get; }
        string BasePath { get; set; } = "Admin";
        List<Type> Components { get; } = new List<Type>();
        List<Action<IContainer>> ContainerOverrides { get; } = new List<Action<IContainer>>();

        public PoetryConfigurator(IContainer container)
        {
            Container = container;
        }

        public PoetryConfigurator WithBasePath(string basePath)
        {
            BasePath = basePath.Trim('/');
            return this;
        }

        public PoetryConfigurator AddComponent<T>()
        {
            Components.Add(typeof(T));
            return this;
        }

        public PoetryConfigurator InjectType<T1, T2>() where T1 : class where T2 : class, T1
        {
            ContainerOverrides.Add(c => c.RegisterTransient<T1, T2>());

            return this;
        }

        public PoetryConfigurator InjectSingleton<T1, T2>() where T1 : class where T2 : class, T1
        {
            ContainerOverrides.Add(c => c.RegisterSingleton<T1, T2>());

            return this;
        }

        public PoetryConfigurator InjectSingleton<T>(T instance) where T : class
        {
            ContainerOverrides.Add(c => c.RegisterSingleton(instance));

            return this;
        }

        public IResolver CreateResolver()
        {
            return Container.CreateResolver();
        }

        public void Done()
        {
            Container.RegisterSingleton<IComponentTypeProvider>(new ComponentTypeProvider(Components));
            new PoetryDependencyInjector().InjectDependencies(Container);

            foreach (var injector in Container.CreateResolver().Resolve<IDependencyInjectorProvider>().GetAll())
            {
                injector.InjectDependencies(Container);
            }

            foreach (var containerOverride in ContainerOverrides)
            {
                containerOverride(Container);
            }
        }
    }
}
