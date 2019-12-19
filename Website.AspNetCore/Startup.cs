using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Poetry.AspNetCore;
using Cloudy.CMS.UI;
using Poetry.UI.AspNetCore;
using Cloudy.CMS;
using Serilog;
using Website.AspNetCore.Models;
using Cloudy.CMS.SingletonSupport;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;

namespace Website.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("test", policy => policy.RequireAssertion(c => false));
            });
            services.AddCloudy(cloudy => cloudy
                .AddComponent<WebsiteComponent>()
                //.WithMongoDatabaseConnectionStringNamed("mongo")
                .WithFileBasedDocuments()
                .AddContentRoute()
                .AddAdmin()
            );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCloudyAdmin(cloudy => cloudy.Unprotect());
            app.UseRouter(r => {
                r.MapContentRoute(null, "{*route:contentroute}", new { controller = "Page" });
            });
        }
    }
}
