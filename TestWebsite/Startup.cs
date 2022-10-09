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

                context.Pages.Add(new Page { Id = new Guid("e6fd53d8-c7de-4355-ae21-c588b2673c5c"), Name = "occaecat ullamco minim" });
                context.Pages.Add(new Page { Id = new Guid("c31836f7-830e-44d3-b231-97d48cf44df3"), Name = "esse ea Excepteur in minim dolore" });
                context.Pages.Add(new Page { Id = new Guid("9ab7ea51-f66a-426e-a18c-51316a396a9e"), Name = "dolor" });
                context.Pages.Add(new Page { Id = new Guid("0c1c40a9-ee61-4071-a1e5-17a2079d882a"), Name = "ut et occaecat ad sit" });
                context.Pages.Add(new Page { Id = new Guid("2738ca18-3061-4858-8bed-8ad418447fc7"), Name = "in" });
                context.Pages.Add(new Page { Id = new Guid("66e44063-a69f-41ac-82bf-220d70709801"), Name = "eiusmod culpa aute Excepteur est" });
                context.Pages.Add(new Page { Id = new Guid("cee2b628-71a8-43e9-88e4-ba51bcf3e940"), Name = "laboris anim ut" });
                context.Pages.Add(new Page { Id = new Guid("0ff35319-ad55-44e0-83b7-f1f8d7b93a41"), Name = "officia veniam, Duis sint ullamco quis" });
                context.Pages.Add(new Page { Id = new Guid("c9ad7601-d0a9-47e4-9515-a2048ba502ad"), Name = "minim aliqua. et esse" });
                context.Pages.Add(new Page { Id = new Guid("5dfd0c64-d1ec-48a4-b50d-5fab3f2606f6"), Name = "Duis non deserunt cillum" });
                context.Pages.Add(new Page { Id = new Guid("cf6053ac-bf4d-417c-90c7-ec8379e23d47"), Name = "anim dolor irure ut" });
                context.Pages.Add(new Page { Id = new Guid("5835789f-91b0-4b74-a399-1099b21a0b27"), Name = "nulla dolore do cillum" });
                context.Pages.Add(new Page { Id = new Guid("4fc870b6-b3d1-443e-acf5-8eab1ff1c4a4"), Name = "nostrud nisi est" });
                context.Pages.Add(new Page { Id = new Guid("2b8c9ecf-1b3c-4059-96ba-972e80ad2a98"), Name = "nostrud ut voluptate sit eu" });
                context.Pages.Add(new Page { Id = new Guid("4b3d6edc-6ce4-481a-b152-85e992490e6e"), Name = "Lorem Duis esse" });
                context.Pages.Add(new Page { Id = new Guid("f3c48571-75ef-4b33-908e-5fa3d5c50901"), Name = "magna" });
                context.Pages.Add(new Page { Id = new Guid("447c9f5f-5ad2-470e-a562-1facb2df5741"), Name = "mollit ea aliqua. elit, sed minim" });
                context.Pages.Add(new Page { Id = new Guid("8f107afa-dc60-4916-8d57-0d262984d450"), Name = "tempor consectetur incididunt mollit" });
                context.Pages.Add(new Page { Id = new Guid("253b28a9-2bf6-4015-b249-f24a38232357"), Name = "cillum eiusmod qui eu" });
                context.Pages.Add(new Page { Id = new Guid("201eb37a-b3f0-4764-848b-e0a964b0ca4d"), Name = "velit" });
                context.Pages.Add(new Page { Id = new Guid("b7e84c3a-97dc-47d5-ae69-abc00a7487b9"), Name = "consectetur nostrud Lorem" });
                context.Pages.Add(new Page { Id = new Guid("6a7cd1b2-e262-48a4-9d80-7f722833a6a0"), Name = "aliquip tempor" });
                context.Pages.Add(new Page { Id = new Guid("28b2affa-6aa9-475d-ad85-a8bc223d965b"), Name = "adipiscing aliquip dolore sit sunt Lorem" });
                context.Pages.Add(new Page { Id = new Guid("2054a8df-b475-41c9-aeee-ad1a0c505eaa"), Name = "aliqua. in veniam, in dolore" });
                context.Pages.Add(new Page { Id = new Guid("7f77f50d-885c-446b-9a61-49d29e3cf2d2"), Name = "ut reprehenderit" });
                context.Pages.Add(new Page { Id = new Guid("b5bf348e-cb21-4714-a331-ddedaf19a0db"), Name = "ut consectetur" });

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
