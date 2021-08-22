using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Website.AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Website.AspNetCore
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCloudy(cloudy => cloudy
                .AddAdmin(admin => admin.Unprotect())
                .AddContext<PageContext>()
            );
            services.AddDbContext<PageContext>(options => options
                .UseSqlServer(
                    Configuration.GetConnectionString("sqlserver") ?? throw new Exception("No sqlserver connection string found in appsettings/env"),
                    options => options.EnableRetryOnFailure()
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCloudyAdminStaticFilesFromPath("../Cloudy.CMS.UI/wwwroot");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCloudyAdminRoutes();
                endpoints.MapGet("/", async c => c.Response.Redirect("/Admin"));
                //endpoints.MapGet("/test/{route:contentroute}", async c => await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute()?.Id}"));
                //endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Blog" });
            });
        }
    }
}
