using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.ApiSupport.RoutingSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Poetry.UI.AspNetCore.ApiSupport
{
    public class ApiMiddleware
    {
        IApiRouter ApiRouter { get; }
        IInstantiator Instantiator { get; }
        RequestDelegate Next { get; }

        public ApiMiddleware(IApiRouter apiRouter, IInstantiator instantiator, RequestDelegate next)
        {
            ApiRouter = apiRouter;
            Instantiator = instantiator;
            Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var routingResult = ApiRouter.Route(httpContext.Request.Path.ToString());

            if (routingResult == null)
            {
                await Next(httpContext);
                return;
            }

            var instance = Instantiator.Instantiate(routingResult.Api.Type);

            IMethodParameterProvider methodParameterProvider = new MethodParameterProvider(new MethodParameterValueProvider(httpContext.Request.Query.Keys.ToDictionary(k => k, k => httpContext.Request.Query[k].First()), () => new StreamReader(httpContext.Request.Body, Encoding.UTF8).ReadToEnd()));

            var parameters = methodParameterProvider.GetParameters(routingResult.Endpoint.Method);

            var result = routingResult.Endpoint.Method.Invoke(instance, parameters);

            if (result != null)
            {
                var mediaType = new MediaTypeHeaderValue("application/json");
                mediaType.CharSet = Encoding.UTF8.WebName;
                httpContext.Response.ContentType = mediaType.ToString();

                using (var stream = new StreamWriter(httpContext.Response.Body))
                using (var json = new JsonTextWriter(stream))
                {
                    new JsonSerializer().Serialize(json, result);
                }
            }
        }
    }
}
