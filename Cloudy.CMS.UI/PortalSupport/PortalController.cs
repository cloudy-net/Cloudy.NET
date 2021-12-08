using Microsoft.AspNetCore.Http;
using Cloudy.CMS.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cloudy.CMS.UI.ScriptSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace Cloudy.CMS.UI.PortalSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class PortalController : Controller
    {
        ITitleProvider TitleProvider { get; }
        IFaviconProvider FaviconProvider { get; }
        IStyleProvider StyleProvider { get; }
        IScriptProvider ScriptProvider { get; }

        public PortalController(ITitleProvider titleProvider, IFaviconProvider faviconProvider, IStyleProvider styleProvider, IScriptProvider scriptProvider)
        {
            TitleProvider = titleProvider;
            FaviconProvider = faviconProvider;
            StyleProvider = styleProvider;
            ScriptProvider = scriptProvider;
        }

        public async Task Index()
        {
            if (!PathEndsInSlash(Request.Path))
            {
                RedirectToPathWithSlash(HttpContext);
                return;
            }

            var basePath = "./files";

            await Response.WriteAsync($"<!DOCTYPE html>\n");
            await Response.WriteAsync($"<html>\n");
            await Response.WriteAsync($"<head>\n");
            await Response.WriteAsync($"    <meta charset=\"utf-8\">\n");
            await Response.WriteAsync($"    <link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&amp;display=swap\"/>\n");
            await Response.WriteAsync($"    <title>{TitleProvider.Title}</title>\n");
            await Response.WriteAsync($"\n");
            await Response.WriteAsync($"    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">\n");
            await Response.WriteAsync($"\n");
            await Response.WriteAsync($"    <link rel=\"icon\" href=\"{FaviconProvider.Favicon}\">\n");
            await Response.WriteAsync($"\n");
            foreach (var style in StyleProvider.GetAll())
            {
                var path = style.Path.StartsWith("http") || style.Path.StartsWith("/") ? style.Path : Path.Combine(basePath, style.Path).Replace('\\', '/');
                await Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{path}\" />\n");
            }
            foreach (var script in ScriptProvider.GetAll())
            {
                var path = script.Path.StartsWith("http") || script.Path.StartsWith("/") ? script.Path : Path.Combine(basePath, script.Path).Replace('\\', '/');
                await Response.WriteAsync($"    <script src=\"{path}\"></script>\n");
            }
            await Response.WriteAsync($"</head>\n");
            await Response.WriteAsync($"<body>\n");
            await Response.WriteAsync($"    <script type=\"module\">\n");
            await Response.WriteAsync($"        import {{ h, render }} from '{Path.Combine(basePath, "lib/preact.module.js").Replace('\\', '/')}';\n");
            await Response.WriteAsync($"        import html from '{Path.Combine(basePath, "html.js").Replace('\\', '/')}';\n");
            await Response.WriteAsync($"        import App from '{Path.Combine(basePath, "app.js").Replace('\\', '/')}';\n");
            await Response.WriteAsync($"        const title = {JsonSerializer.Serialize(TitleProvider.Title)};\n");
            await Response.WriteAsync($"        render(html`<${{App}} title=${{title}} />`, document.body);\n");
            await Response.WriteAsync($"    </script>\n");
            await Response.WriteAsync($"</body>\n");
            await Response.WriteAsync($"</html>\n");
        }

        bool PathEndsInSlash(PathString path)
        {
            return path.Value.EndsWith("/", StringComparison.Ordinal);
        }

        void RedirectToPathWithSlash(HttpContext context)
        {
            Response.StatusCode = StatusCodes.Status301MovedPermanently;
            var request = context.Request;
            var redirect = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path + "/", request.QueryString);
            Response.Headers[HeaderNames.Location] = redirect;
        }
    }
}
