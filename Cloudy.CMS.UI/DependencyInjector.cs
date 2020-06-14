using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Cloudy.CMS.UI.IdentitySupport;
using Cloudy.CMS.UI.PortalSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.UI.AppSupport;
using Cloudy.CMS.UI.ScriptSupport;
using Cloudy.CMS.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IFaviconProvider, FaviconProvider>();
            services.AddSingleton<ITitleProvider, TitleProvider>();
            services.AddSingleton<IAppCreator, AppCreator>();
            services.AddSingleton<IAppProvider, AppProvider>();
            services.AddSingleton<IScriptProvider, ScriptProvider>();
            services.AddSingleton<IScriptCreator, ScriptCreator>();
            services.AddSingleton<IStyleProvider, StyleProvider>();
            services.AddSingleton<IStyleCreator, StyleCreator>();
            services.AddSingleton<IAppProvider, AppProvider>();
            services.AddSingleton<IMemberExpressionFromExpressionExtractor, MemberExpressionFromExpressionExtractor>();
            services.AddSingleton<IUrlProvider, UrlProvider>();
            services.AddSingleton<ITitleProvider, TitleProvider>();
            services.AddSingleton<IFaviconProvider, FaviconProvider>();
            services.AddSingleton<IPortalPageRenderer, PortalPageRenderer>();
            services.AddSingleton<IStaticFilesBasePathProvider, StaticFilesBasePathProvider>();
            services.AddSingleton<IContentFormIdGenerator, ContentFormIdGenerator>();
            services.AddSingleton<IPluralizer, Pluralizer>();
            services.AddSingleton<IHumanizer, Humanizer>();
            services.AddSingleton<INameExpressionParser, NameExpressionParser>();
            services.AddSingleton<IPipelineBuilder, PipelineBuilder>();
            services.AddSingleton<ILoginPipelineBuilder, LoginPipelineBuilder>();
            services.AddSingleton<ILoginPageRenderer, LoginPageRenderer>();
            services.AddSingleton<IListActionModuleCreator, ListActionModuleCreator>();
            services.AddSingleton<IListActionModuleProvider, ListActionModuleProvider>();
            services.AddSingleton<IContentTypeActionModuleCreator, ContentTypeActionModuleCreator>();
            services.AddSingleton<IContentTypeActionModuleProvider, ContentTypeActionModuleProvider>();
        }
    }
}
