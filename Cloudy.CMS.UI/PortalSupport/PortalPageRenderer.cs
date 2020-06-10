using Microsoft.AspNetCore.Http;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cloudy.CMS.UI.ScriptSupport;

namespace Cloudy.CMS.UI.PortalSupport
{
    public class PortalPageRenderer : IPortalPageRenderer
    {
        ITitleProvider TitleProvider { get; }
        IFaviconProvider FaviconProvider { get; }
        IStaticFilesBasePathProvider StaticFilesBasePathProvider { get; }
        IStyleProvider StyleProvider { get; }
        IScriptProvider ScriptProvider { get; }

        public PortalPageRenderer(ITitleProvider titleProvider, IFaviconProvider faviconProvider, IStaticFilesBasePathProvider staticFilesBasePathProvider, IStyleProvider styleProvider, IScriptProvider scriptProvider)
        {
            TitleProvider = titleProvider;
            FaviconProvider = faviconProvider;
            StaticFilesBasePathProvider = staticFilesBasePathProvider;
            StyleProvider = styleProvider;
            ScriptProvider = scriptProvider;
        }

        public async Task RenderPageAsync(HttpContext context)
        {
            var basePath = StaticFilesBasePathProvider.StaticFilesBasePath;

            if (basePath.StartsWith("./"))
            {
                basePath = Path.Combine(context.Request.PathBase.Value, basePath.Substring(2)).Replace('\\', '/');
            }

            await context.Response.WriteAsync($"<!DOCTYPE html>\n");
            await context.Response.WriteAsync($"<html>\n");
            await context.Response.WriteAsync($"<head>\n");
            await context.Response.WriteAsync($"    <meta charset=\"utf-8\">\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&amp;display=swap\"/>\n");
            await context.Response.WriteAsync($"    <title>{TitleProvider.Title}</title>\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <link rel=\"icon\" href=\"{FaviconProvider.Favicon}\">\n");
            await context.Response.WriteAsync($"\n");
            foreach (var style in StyleProvider.GetAll())
            {
                var path = style.Path.StartsWith("http") || style.Path.StartsWith("/") ? style.Path : Path.Combine(basePath, style.Path).Replace('\\', '/');
                await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{path}\" />\n");
            }
            foreach (var script in ScriptProvider.GetAll())
            {
                var path = script.Path.StartsWith("http") || script.Path.StartsWith("/") ? script.Path : Path.Combine(basePath, script.Path).Replace('\\', '/');
                await context.Response.WriteAsync($"    <script src=\"{path}\"></script>\n");
            }
            await context.Response.WriteAsync($"</head>\n");
            await context.Response.WriteAsync($"<body>\n");
            await context.Response.WriteAsync($"    <script type=\"module\">\n");
            await context.Response.WriteAsync($"        import Portal from '{Path.Combine(basePath, "portal.js").Replace('\\', '/')}';\n");
            await context.Response.WriteAsync($"        new Portal().setTitle('{TitleProvider.Title}');\n");
            await context.Response.WriteAsync($"    </script>\n");
            await context.Response.WriteAsync($"</body>\n");
            await context.Response.WriteAsync($"</html>\n");
        }
    }
}
