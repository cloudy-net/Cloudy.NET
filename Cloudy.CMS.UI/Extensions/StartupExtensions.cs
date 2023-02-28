using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Cloudy.CMS.UI;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.AspNetCore.Builder
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddAdmin(this CloudyConfigurator configurator)
        {
            return configurator.AddAdmin(admin => { });
        }

        public static CloudyConfigurator AddAdmin(this CloudyConfigurator configurator, Action<CloudyAdminConfigurator> admin)
        {
            configurator.AddComponent(Assembly.GetExecutingAssembly());

            configurator.Services.AddMvc().AddApplicationPart(Assembly.GetExecutingAssembly());

            var options = new CloudyAdminOptions();
            admin(new CloudyAdminConfigurator(options));
            configurator.Services.AddSingleton(options);

            if (options.Unprotect)
            {
                configurator.Services.Configure<AuthorizationOptions>(o => o.AddPolicy("adminarea", builder => builder.RequireAssertion(a => true)));
            }

            return configurator;
        }

        public static IApplicationBuilder UseCloudy(this IApplicationBuilder applicationBuilder) => applicationBuilder
            .UseRewriter(new RewriteOptions().AddRewrite("^Admin/(?!api).*", "/Admin/Index", true));
    }
}