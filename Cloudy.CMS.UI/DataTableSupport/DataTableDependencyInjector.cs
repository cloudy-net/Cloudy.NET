using Poetry.UI.DataTableSupport.BackendSupport;
using Poetry.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.DataTableSupport
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
