using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS;
using Cloudy.CMS;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Cloudy.CMS.AspNetCore;
using Cloudy.CMS.UI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.InitializerSupport;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.UI.PortalSupport;
using Cloudy.CMS.UI;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;

namespace Microsoft.AspNetCore.Builder
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddAdmin(this CloudyConfigurator configurator)
        {
            configurator.AddComponent<CloudyAdminComponent>();
            configurator.Services.AddSingleton(new CloudyAdminOptions());

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

            var containerName = $"v-{version.Major}-{version.Minor}";

            if (version.Build != 0)
            {
                containerName += $"-{version.Build}";
            }

            app.UseCloudyAdminStaticFilesFromUrl($"https://cloudycmsui.blob.core.windows.net/{containerName}");
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