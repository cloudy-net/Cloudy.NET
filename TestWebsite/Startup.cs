using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using TestWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.Routing;
using Cloudy.CMS.UI;
using System.Linq;

namespace TestWebsite
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

            // seed with some data
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<PageContext>();
                var random = new Random();
                var lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.".Split(' ');

                for (var i = 0; i < 30; i++)
                {
                    context.Add(new Page { Name = string.Join(" ", lipsum.OrderBy(t => random.Next()).Take(random.Next(5 + 2))).TrimEnd(',', '.') });
                }

                context.SaveChanges();
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
                
                endpoints.MapGet("/pages/{route:contentroute}", async c => 
                    await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute<Page>().Name}")
                );
                
                endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Index" });
            });
        }
    }
}
