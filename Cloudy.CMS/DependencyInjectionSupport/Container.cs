using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.AspNetCore.DependencyInjectionSupport
{
    public class Container : IContainer
    {
        public IServiceCollection ServiceCollection { get; }

        public Container(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public void RegisterSingleton<T1, T2>() where T1 : class where T2 : class, T1
        {
            ServiceCollection.AddSingleton<T1, T2>();
        }

        public void RegisterSingleton<T>(T instance) where T : class
        {
            ServiceCollection.AddSingleton(instance);
        }

        public void RegisterTransient<T1, T2>() where T1 : class where T2 : class, T1
        {
            ServiceCollection.AddTransient<T1, T2>();
        }

        public void RegisterTransient(Type from, Type to)
        {
            ServiceCollection.AddTransient(from, to);
        }

        public IResolver CreateResolver()
        {
            return new Resolver(ServiceCollection.BuildServiceProvider());
        }
    }
}
