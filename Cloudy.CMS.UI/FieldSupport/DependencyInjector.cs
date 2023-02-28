using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI.FieldSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IFieldCreator, FieldCreator>();
            services.AddSingleton<IFieldProvider, FieldProvider>();
            services.AddScoped<IFieldFriendlyValueProvider, FieldFriendlyValueProvider>();
        }
    }
}
