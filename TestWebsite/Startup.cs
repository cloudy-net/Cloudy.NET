using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Website.AspNetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.Routing;
using Cloudy.CMS.UI;

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
            services.AddRazorPages().AddApplicationPart(typeof(CloudyUIAssemblyHandle).Assembly);
            services.AddMvc();
            services.AddCloudy(cloudy => cloudy
                .AddAdmin(admin => admin.Unprotect())
                .AddContext<PageContext>()
            );
            services.AddDbContext<PageContext>(options => options
                .UseInMemoryDatabase("cloudytest")
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx => ctx.Context.Response.Headers.Append("Cache-Control", $"no-cache")
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapGet("/", async c => c.Response.Redirect("/Admin"));
                endpoints.MapGet("/pages", async c => await c.Response.WriteAsJsonAsync(c.RequestServices.GetService<PageContext>().Pages));
                endpoints.MapGet("/pages/{route:contentroute}", async c => await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute<Page>().Name}"));
                endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Index" });
            });
        }
    }
}
