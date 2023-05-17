using Cloudy.NET.DependencyInjectionSupport;
using Cloudy.NET.UI.FormSupport.ChangeHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.NET.UI.FormSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IEntityNavigator, EntityNavigator>();
            services.AddSingleton<ISimpleChangeHandler, SimpleChangeHandler>();
            services.AddSingleton<IBlockTypeChangeHandler, BlockTypeChangeHandler>();
            services.AddSingleton<IEmbeddedBlockListHandler, EmbeddedBlockListHandler>();
        }
    }
}
