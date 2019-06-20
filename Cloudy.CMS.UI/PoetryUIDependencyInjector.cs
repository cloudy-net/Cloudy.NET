using Poetry.DependencyInjectionSupport;
using Poetry.UI.ApiSupport;
using Poetry.UI.ApiSupport.RoutingSupport;
using Poetry.UI.AppSupport;
using Poetry.UI.EmbeddedResourceSupport;
using Poetry.UI.PortalSupport;
using Poetry.UI.RoutableResourceSupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI
{
    public class PoetryUIDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<IApiRouter, ApiRouter>();
            container.RegisterSingleton<IApiCreator, ApiCreator>();
            container.RegisterSingleton<IApiProvider, ApiProvider>();
            container.RegisterSingleton<IEndpointCreator, EndpointCreator>();
            container.RegisterSingleton<IFaviconProvider, FaviconProvider>();
            container.RegisterSingleton<ITitleProvider, TitleProvider>();
            container.RegisterSingleton<IAppCreator, AppCreator>();
            container.RegisterSingleton<IAppProvider, AppProvider>();
            container.RegisterSingleton<IScriptProvider, ScriptProvider>();
            container.RegisterSingleton<IScriptCreator, ScriptCreator>();
            container.RegisterSingleton<IStyleProvider, StyleProvider>();
            container.RegisterSingleton<IStyleCreator, StyleCreator>();
            container.RegisterSingleton<IRoutableResourceProvider, RoutableResourceProvider>();
            container.RegisterSingleton<IRoutableResourceCreator, RoutableResourceCreator>();
            container.RegisterSingleton<IEmbeddedResourceRouter, EmbeddedResourceRouter>();
            container.RegisterSingleton<IAppProvider, AppProvider>();
            container.RegisterSingleton<IMainPageGenerator, MainPageGenerator>();
        }
    }
}
