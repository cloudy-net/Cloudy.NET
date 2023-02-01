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
using TestWebsite.Factories;

namespace TestWebsite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddCloudy(cloudy => cloudy
                .AddAdmin(admin => admin.Unprotect())
                .AddAzureMediaPicker()
                .AddContext<PageContext>()
            );
#pragma warning restore CS0618 // Type or member is obsolete
            services.AddDbContext<PageContext>(options => options
                .UseInMemoryDatabase("cloudytest")
            );

            services.AddSingleton<IColorFactory, ColorFactory>();
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

                context.SimpleKeyTests.Add(new SimpleKeyTest { Id = new Guid("042f0213-6e95-4b23-b924-e43bb51c219d") });
                context.SimpleKeyTests.Add(new SimpleKeyTest { Id = new Guid("51e36eda-12ce-41f7-893e-4a475a2b7116"), RelatedPage = new Tuple<Guid>(new Guid("042f0213-6e95-4b23-b924-e43bb51c219d")), RelatedPage2 = new Guid("042f0213-6e95-4b23-b924-e43bb51c219d"), RelatedPage3 = new Guid("042f0213-6e95-4b23-b924-e43bb51c219d") });

                context.CompositeKeyTests.Add(new CompositeKeyTest { FirstPrimaryKey = new Guid("69379a33-7a76-4309-b73f-2ff1ac83da25"), SecondPrimaryKey = 1 });
                context.CompositeKeyTests.Add(new CompositeKeyTest { FirstPrimaryKey = new Guid("3fdb600b-e801-4588-9f6d-cf03df8180d8"), SecondPrimaryKey = 2, RelatedObject = new Tuple<Guid, int>(new Guid("69379a33-7a76-4309-b73f-2ff1ac83da25"), 1) });

                context.Pages.Add(new Page { Id = new Guid("e6fd53d8-c7de-4355-ae21-c588b2673c5c"), Name = "occaecat ullamco minim", RelatedPageId = new Guid("66e44063-a69f-41ac-82bf-220d70709801"), UrlSegment = "lorem" });
                context.Pages.Add(new Page { Id = new Guid("c31836f7-830e-44d3-b231-97d48cf44df3"), Name = "esse ea Excepteur in minim dolore" });
                context.Pages.Add(new Page { Id = new Guid("0c1c40a9-ee61-4071-a1e5-17a2079d882a"), Name = "ut et occaecat ad sit" });
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
                context.Pages.Add(new Page { Id = new Guid("447c9f5f-5ad2-470e-a562-1facb2df5741"), Name = "mollit ea aliqua. elit, sed minim" });
                context.Pages.Add(new Page { Id = new Guid("8f107afa-dc60-4916-8d57-0d262984d450"), Name = "tempor consectetur incididunt mollit" });
                context.Pages.Add(new Page { Id = new Guid("253b28a9-2bf6-4015-b249-f24a38232357"), Name = "cillum eiusmod qui eu" });
                context.Pages.Add(new Page { Id = new Guid("b7e84c3a-97dc-47d5-ae69-abc00a7487b9"), Name = "consectetur nostrud Lorem" });
                context.Pages.Add(new Page { Id = new Guid("6a7cd1b2-e262-48a4-9d80-7f722833a6a0"), Name = "aliquip tempor" });
                context.Pages.Add(new Page { Id = new Guid("28b2affa-6aa9-475d-ad85-a8bc223d965b"), Name = "adipiscing aliquip dolore sit sunt Lorem" });
                context.Pages.Add(new Page { Id = new Guid("2054a8df-b475-41c9-aeee-ad1a0c505eaa"), Name = "aliqua. in veniam, in dolore" });
                context.Pages.Add(new Page { Id = new Guid("7f77f50d-885c-446b-9a61-49d29e3cf2d2"), Name = "ut reprehenderit" });

                context.SaveChanges();
            }

            app.UseStaticFiles(new StaticFileOptions().MustValidate());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
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
