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
using Microsoft.AspNetCore.Routing.Matching;
using Cloudy.CMS.Routing;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Website.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddControllersWithViews();
            services.AddCloudy(cloudy => cloudy
                //.AddComponent<WebsiteComponent>()
                //.WithMongoDatabaseConnectionStringNamed("mongo")
                .WithFileBasedDocuments()
                .AddContentRoute()
                .AddAdmin()
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCloudyAdmin(cloudy => cloudy.WithStaticFilesFrom(new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "../Cloudy.CMS.UI/wwwroot"))).Unprotect());
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/test/{route:contentroute}", async c => await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute()?.Id}"));
                endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Blog" });
            });
        }
    }
}
