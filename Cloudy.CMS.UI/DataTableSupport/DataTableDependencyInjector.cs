using Cloudy.CMS.UI.DataTableSupport.BackendSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.DataTableSupport
{
    public class DataTableDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<IBackendCreator, BackendCreator>();
            container.RegisterSingleton<IBackendProvider, BackendProvider>();
        }
    }
}
