using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DependencyInjectionSupport
{
    public interface IContainer
    {
        void RegisterSingleton<T1, T2>() where T1 : class where T2 : class, T1;
        void RegisterSingleton<T>(T instance) where T : class;
        void RegisterTransient<T1, T2>() where T1 : class where T2 : class, T1;
        void RegisterTransient(Type from, Type to);
        IResolver CreateResolver();
    }
}
