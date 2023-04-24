using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.UI.FormSupport.ChangeHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI.FormSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IEntityNavigator, EntityNavigator>();
            services.AddSingleton<ISimpleChangeHandler, SimpleChangeHandler>();
            services.AddSingleton<IBlockTypeChangeHandler, BlockTypeChangeHandler>();
            services.AddScoped<IListTracker, ListTracker>();
        }
    }
}
