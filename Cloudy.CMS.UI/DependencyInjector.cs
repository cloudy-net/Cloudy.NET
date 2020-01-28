using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.PortalSupport;
using Poetry.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<ITitleProvider, TitleProvider>();
            container.RegisterSingleton<IFaviconProvider, FaviconProvider>();
            container.RegisterSingleton<IPortalPageRenderer, PortalPageRenderer>();
            container.RegisterSingleton<IStaticFilesBasePathProvider, StaticFilesBasePathProvider>();
            container.RegisterSingleton<IPluralizer, Pluralizer>();
            container.RegisterSingleton<IHumanizer, Humanizer>();
        }
    }
}
