using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.InitializerSupport
{
    public class InitializerBootstrapper : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    foreach (var initializer in scope.ServiceProvider.GetRequiredService<IInitializerProvider>().GetAll())
                    {
                        initializer.InitializeAsync().GetAwaiter().GetResult();
                    }
                }
                next(app);
            };
        }
    }
}
