using Cloudy.CMS.UI.PortalSupport;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public class LoginPageRenderer : ILoginPageRenderer
    {
        ITitleProvider TitleProvider { get; }
        IFaviconProvider FaviconProvider { get; }
        IStaticFilesBasePathProvider StaticFilesBasePathProvider { get; }
        IStyleProvider StyleProvider { get; }

        public LoginPageRenderer(ITitleProvider titleProvider, IFaviconProvider faviconProvider, IStaticFilesBasePathProvider staticFilesBasePathProvider, IStyleProvider styleProvider)
        {
            TitleProvider = titleProvider;
            FaviconProvider = faviconProvider;
            StaticFilesBasePathProvider = staticFilesBasePathProvider;
            StyleProvider = styleProvider;
        }

        public async Task RenderAsync(HttpContext context)
        {
            await context.Response.WriteAsync($"<!DOCTYPE html>\n");
            await context.Response.WriteAsync($"<html>\n");
            await context.Response.WriteAsync($"<head>\n");
            await context.Response.WriteAsync($"    <meta charset=\"utf-8\">\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&amp;display=swap\"/>\n");
            await context.Response.WriteAsync($"    <base href=\"../\">\n");
            await context.Response.WriteAsync($"    <title>{TitleProvider.Title}</title>\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <link rel=\"icon\" href=\"{FaviconProvider.Favicon}\">\n");
            await context.Response.WriteAsync($"\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "portal.css").Replace('\\', '/')}\" />\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "login.css").Replace('\\', '/')}\" />\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "FormSupport/form-elements.css").Replace('\\', '/')}\" />\n");
            await context.Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "NotificationSupport/notification-manager.css").Replace('\\', '/')}\" />\n");
            await context.Response.WriteAsync($"</head>\n");
            await context.Response.WriteAsync($"<body>\n");
            await context.Response.WriteAsync($"    <script type=\"module\">\n");
            await context.Response.WriteAsync($"        import Login from '{Path.Combine(StaticFilesBasePathProvider.StaticFilesBasePath, "login.js").Replace('\\', '/')}';\n");
            await context.Response.WriteAsync($"        window.cloudyPath = '{StaticFilesBasePathProvider.StaticFilesBasePath}'\n");
            await context.Response.WriteAsync($"        new Login().setTitle('Login to {TitleProvider.Title}');\n");
            await context.Response.WriteAsync($"    </script>\n");
            await context.Response.WriteAsync($"</body>\n");
            await context.Response.WriteAsync($"</html>\n");
        }
    }
}
