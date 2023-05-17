using Cloudy.NET.DependencyInjectionSupport;
using Cloudy.NET.UI.List;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.NET.UI.FieldSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IFieldCreator, FieldCreator>();
            services.AddSingleton<IFieldProvider, FieldProvider>();
            services.AddScoped<IColumnValueProvider, ColumnValueProvider>();
        }
    }
}
