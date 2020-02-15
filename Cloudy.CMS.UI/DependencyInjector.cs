using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Cloudy.CMS.UI.IdentitySupport;
using Cloudy.CMS.UI.PortalSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.AppSupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI
{
    public class DependencyInjector : IDependencyInjector
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
            container.RegisterSingleton<IMemberExpressionFromExpressionExtractor, MemberExpressionFromExpressionExtractor>();
            container.RegisterSingleton<IUrlProvider, UrlProvider>();
            container.RegisterSingleton<ITitleProvider, TitleProvider>();
            container.RegisterSingleton<IFaviconProvider, FaviconProvider>();
            container.RegisterSingleton<IPortalPageRenderer, PortalPageRenderer>();
            container.RegisterSingleton<IStaticFilesBasePathProvider, StaticFilesBasePathProvider>();
            container.RegisterSingleton<IPluralizer, Pluralizer>();
            container.RegisterSingleton<IHumanizer, Humanizer>();
            container.RegisterSingleton<INameExpressionParser, NameExpressionParser>();
            container.RegisterSingleton<IPipelineBuilder, PipelineBuilder>();
            container.RegisterSingleton<ILoginPipelineBuilder, LoginPipelineBuilder>();
            container.RegisterSingleton<ILoginPageRenderer, LoginPageRenderer>();
            container.RegisterSingleton<IListActionModuleCreator, ListActionModuleCreator>();
            container.RegisterSingleton<IListActionModuleProvider, ListActionModuleProvider>();
        }
    }
}
