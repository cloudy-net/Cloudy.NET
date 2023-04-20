using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI.FormSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IFormEntityUpdater, FormEntityUpdater>();
        }
    }
}
