using Microsoft.AspNetCore.Http;
using Poetry.UI;
using Poetry.EmbeddedResourceSupport;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Poetry.UI.AppSupport;
using Poetry.UI.EmbeddedResourceSupport;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace Poetry.UI.AspNetCore.EmbeddedResourceSupport
{
    public class EmbeddedResourcesMiddleware
    {
        public static IEnumerable<string> HostnamesActivatedForDevelopmentMode { get; set; }

        IEmbeddedResourceRouter EmbeddedResourceRouter { get; }
        IEmbeddedResourceProvider EmbeddedResourceProvider { get; }
        IHostingEnvironment HostingEnvironment { get; }
        string Prefix { get; }
        RequestDelegate Next { get; }

        public EmbeddedResourcesMiddleware(IBasePathProvider basePathProvider, IEmbeddedResourceRouter embeddedResourceRouter, IEmbeddedResourceProvider embeddedResourceProvider, IHostingEnvironment hostingEnvironment, RequestDelegate next)
        {
            EmbeddedResourceRouter = embeddedResourceRouter;
            EmbeddedResourceProvider = embeddedResourceProvider;
            HostingEnvironment = hostingEnvironment;
            Prefix = $"/{basePathProvider.BasePath}/";
            Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.ToString();

            if (!path.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                await Next(httpContext);
                return;
            }

            path = path.Substring(Prefix.Length);

            var resource = EmbeddedResourceRouter.Route(path);

            if(resource == null)
            {
                await Next(httpContext);
                return;
            }

            if (path.EndsWith(".js"))
            {
                var mediaType = new MediaTypeHeaderValue("application/javascript");
                mediaType.CharSet = Encoding.UTF8.WebName;
                httpContext.Response.ContentType = mediaType.ToString();
            }
            else if (path.EndsWith(".css"))
            {
                var mediaType = new MediaTypeHeaderValue("text/css");
                mediaType.CharSet = Encoding.UTF8.WebName;
                httpContext.Response.ContentType = mediaType.ToString();
            }
            else
            {
                var mediaType = new MediaTypeHeaderValue("application/octet-stream");
                mediaType.CharSet = Encoding.UTF8.WebName;
                httpContext.Response.ContentType = mediaType.ToString();
            }

            if (HostnamesActivatedForDevelopmentMode != null && HostnamesActivatedForDevelopmentMode.Contains(httpContext.Request.Host.Host))
            {
                var mappedPath = Path.Combine(HostingEnvironment.ContentRootPath, "..", path.Replace('/', '\\'));

                if (File.Exists(mappedPath))
                {
                    using (var read = File.OpenRead(mappedPath))
                    {
                        await read.CopyToAsync(httpContext.Response.Body);
                    }

                    return;
                }
            }

            using(var read = EmbeddedResourceProvider.Open(resource))
            {
                await read.CopyToAsync(httpContext.Response.Body);
            }
        }
    }
}
