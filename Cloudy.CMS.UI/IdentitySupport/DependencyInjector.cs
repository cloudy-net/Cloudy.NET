using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<INormalizer, Normalizer>();
            services.AddSingleton<ILoginPageBrandingPathProvider>(new LoginPageBrandingPathProvider(null));
        }
    }
}
