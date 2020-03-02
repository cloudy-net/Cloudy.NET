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
using Cloudy.CMS.AspNetCore;
using Cloudy.CMS.UI;
using Cloudy.CMS.UI.AspNetCore;
using Cloudy.CMS;
using Cloudy.CMS.SingletonSupport;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Cloudy.CMS.Routing;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Cloudy.CMS.UI.IdentitySupport;
using Website.AspNetCore.DemoSupport;

namespace Website.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDemoPageRenderer, DemoPageRenderer>();
            services.AddMvc();
            services.AddCloudy(cloudy => cloudy
                //.WithMongoDatabaseConnectionStringNamed("mongo")
                .WithFileBasedDocuments()
                .AddAdmin()
            );
            services.AddCloudyIdentity();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCloudyAdmin(cloudy => 
                cloudy
                    .WithStaticFilesFrom(new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "../Cloudy.CMS.UI/wwwroot")))
                    .WithHelpSectionsFrom("https://localhost:44369/help-sections")
                    .Unprotect()
            );
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", app.ApplicationServices.GetService<IDemoPageRenderer>().RenderAsync);
                endpoints.MapGet("/test/{route:contentroute}", async c => await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute()?.Id}"));
                endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Blog" });
            });
        }
    }
}
