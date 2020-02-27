using Cloudy.CMS.UI.PortalSupport;
using Microsoft.AspNetCore.Http;
using Poetry.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.DemoSupport
{
    public class DemoPageRenderer : IDemoPageRenderer
    {
        IStaticFilesBasePathProvider StaticFilesBasePathProvider { get; }
        IStyleProvider StyleProvider { get; }

        public DemoPageRenderer(IStaticFilesBasePathProvider staticFilesBasePathProvider, IStyleProvider styleProvider)
        {
            StaticFilesBasePathProvider = staticFilesBasePathProvider;
            StyleProvider = styleProvider;
        }

        public async Task RenderAsync(HttpContext context)
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
            await context.Response.WriteAsync($"    <title>Cloudy CMS</title>\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <link rel=\"icon\" href=\"data:image/png;base64,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\">\n");
            await context.Response.WriteAsync($"\n");
            foreach (var style in StyleProvider.GetAll())
            {
                await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine("Admin/", basePath, style.Path).Replace('\\', '/')}\" />\n");
            }
            await context.Response.WriteAsync($"</head>\n");
            await context.Response.WriteAsync($"<body>\n");
            await context.Response.WriteAsync($"    <script type=\"module\" src=\"/demo/demo.js\"></script>\n");
            await context.Response.WriteAsync($" </body>\n");
            await context.Response.WriteAsync($" </html>\n");
        }
    }
}
