using Cloudy.CMS.UI.ContentAppSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.PortalSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<Poetry.UI.PortalSupport.ITitleProvider, TitleProvider>();
            container.RegisterSingleton<Poetry.UI.PortalSupport.IFaviconProvider, FaviconProvider>();
            container.RegisterSingleton<IPluralizer, Pluralizer>();
            container.RegisterSingleton<IHumanizer, Humanizer>();
        }
    }
}
