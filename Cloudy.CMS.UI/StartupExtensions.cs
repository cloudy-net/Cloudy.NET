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

        public static void UseCloudyAdminStaticFiles(this IApplicationBuilder app)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            if (version.Equals(new Version(1, 0, 0, 0)))
            {
                throw new Exception("It seems you have linked Cloudy CMS through a project reference. Please add something like app.UseCloudyAdminStaticFilesFromPath(\"../cloudy-cms/Cloudy.CMS.UI/wwwroot\") to find your local static files");
            }

            app.UseCloudyAdminStaticFilesWithVersion(version);
        }

        public static void UseCloudyAdminStaticFilesWithVersion(this IApplicationBuilder app, Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version), "Cloudy CMS UI was instructed to link static files based on a Version, but that version was null");
            }

            var containerName = $"v-{version.Major}-{version.Minor}-{version.Build}";

            app.UseCloudyAdminStaticFilesFromUrl($"https://cloudyui.blob.core.windows.net/{containerName}");
        }

        public static void UseCloudyAdminStaticFilesFromUrl(this IApplicationBuilder app, string url)
        {
            url = url.TrimEnd('/');
            app.UseRewriter(new RewriteOptions().AddRedirect("Admin/files/(.*)", $"{url}/$1"));
        }

        public static void UseCloudyAdminStaticFilesFromPath(this IApplicationBuilder app, string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(app.ApplicationServices.GetService<IWebHostEnvironment>().ContentRootPath, path);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Admin/files",
                FileProvider = new PhysicalFileProvider(path),
                OnPrepareResponse = context => context.Context.Response.Headers["Cache-Control"] = "no-cache"
            });
        }

        public static void MapCloudyAdminRoutes(this IEndpointRouteBuilder configure)
        {
            configure.MapAreaControllerRoute(null, "Cloudy.CMS", "Admin/{controller}/{action}", new { controller = "Portal", action = "Index" });
        }
    }
}