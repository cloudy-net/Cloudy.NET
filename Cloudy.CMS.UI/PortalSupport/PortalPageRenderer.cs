using Microsoft.AspNetCore.Http;
using Poetry.ComponentSupport;
using Poetry.UI.StyleSupport;
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
        IStyleProvider StyleProvider { get; }

        public PortalPageRenderer(ITitleProvider titleProvider, IFaviconProvider faviconProvider, IStaticFilesBasePathProvider staticFilesBasePathProvider, IStyleProvider styleProvider)
        {
            TitleProvider = titleProvider;
            FaviconProvider = faviconProvider;
            StaticFilesBasePathProvider = staticFilesBasePathProvider;
            StyleProvider = styleProvider;
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
            foreach(var style in StyleProvider.GetAll())
            {
                await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, style.Path).Replace('\\', '/')}\" />\n");
            }
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
