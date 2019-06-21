using Cloudy.CMS.AspNetCore.ContentControllerSupport;
using Cloudy.CMS.ContentControllerSupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.AppSupport;
using Poetry.UI.AspNetCore.AuthorizationSupport;
using Poetry.UI.PortalSupport;
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
            container.RegisterSingleton<IFaviconProvider, FaviconProvider>();
            container.RegisterSingleton<ITitleProvider, TitleProvider>();
            container.RegisterSingleton<IAppCreator, AppCreator>();
            container.RegisterSingleton<IAppProvider, AppProvider>();
            container.RegisterSingleton<IScriptProvider, ScriptProvider>();
            container.RegisterSingleton<IScriptCreator, ScriptCreator>();
            container.RegisterSingleton<IStyleProvider, StyleProvider>();
            container.RegisterSingleton<IStyleCreator, StyleCreator>();
            container.RegisterSingleton<IAppProvider, AppProvider>();
            container.RegisterSingleton<IMainPageGenerator, MainPageGenerator>();
            container.RegisterSingleton<IMemberExpressionFromExpressionExtractor, MemberExpressionFromExpressionExtractor>();
            container.RegisterSingleton<IUrlProvider, UrlProvider>();
            container.RegisterSingleton<IControllerProvider, ControllerProvider>();
            container.RegisterSingleton<IContentControllerMatchCreator, ContentControllerMatchCreator>();
            container.RegisterSingleton<IUIAuthorizationPolicyProvider, UIAuthorizationPolicyProvider>();
        }
    }
}
