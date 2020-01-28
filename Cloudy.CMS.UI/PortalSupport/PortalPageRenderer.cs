using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.PortalSupport
{
    public class PortalPageRenderer : IPortalPageRenderer
    {
        ITitleProvider TitleProvider { get; }
        IFaviconProvider FaviconProvider { get; }
        IStaticFilesBasePathProvider StaticFilesBasePathProvider { get; }

        public PortalPageRenderer(ITitleProvider titleProvider, IFaviconProvider faviconProvider, IStaticFilesBasePathProvider staticFilesBasePathProvider)
        {
            TitleProvider = titleProvider;
            FaviconProvider = faviconProvider;
            StaticFilesBasePathProvider = staticFilesBasePathProvider;
        }

        public async Task RenderPageAsync(HttpContext context)
        {
            await context.Response.WriteAsync($"<!DOCTYPE html>\n");
            await context.Response.WriteAsync($"<html>\n");
            await context.Response.WriteAsync($"<head>\n");
            await context.Response.WriteAsync($"    <meta charset=\"utf-8\">\n");
            await context.Response.WriteAsync($"    <title>{TitleProvider.Title}</title>\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <link rel=\"icon\" href=\"{FaviconProvider.Favicon}\">\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "portal.css").Replace('\\', '/')}\" />\n");
            await context.Response.WriteAsync($"</head>\n");
            await context.Response.WriteAsync($"<body>\n");
            await context.Response.WriteAsync($"    <script type=\"module\">\n");
            await context.Response.WriteAsync($"        import Portal from '{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "portal.js").Replace('\\', '/')}';\n");
            await context.Response.WriteAsync($"        new Portal();\n");
            await context.Response.WriteAsync($"    </script>\n");
            await context.Response.WriteAsync($"</body>\n");
            await context.Response.WriteAsync($"</html>\n");
        }
    }
}
