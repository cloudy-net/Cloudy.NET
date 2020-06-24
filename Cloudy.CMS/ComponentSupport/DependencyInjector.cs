using Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ComponentSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IComponentCreator, ComponentCreator>();
            services.AddSingleton<IComponentProvider, ComponentProvider>();
            services.AddSingleton<IMissingComponentAttributeChecker, MissingComponentAttributeChecker>();
            services.AddSingleton<IDuplicateComponentIdChecker, DuplicateComponentIdChecker>();
            services.AddSingleton<IMultipleComponentsInSingleAssemblyChecker, MultipleComponentsInSingleAssemblyChecker>();
        }
    }
}
