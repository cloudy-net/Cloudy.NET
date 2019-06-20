using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Poetry.UI;
using Poetry.UI.AspNetCore.AuthorizationSupport;
using Poetry.UI.PortalSupport;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Poetry.UI.AspNetCore.PortalSupport
{
    public class MainPageMiddleware
    {
        IAuthorizationService AuthorizationService { get; }
        IUIAuthorizationPolicyProvider UIAuthorizationPolicyProvider { get; }
        IMainPageGenerator MainPageGenerator { get; }
        string Prefix1 { get; }
        string Prefix2 { get; }
        RequestDelegate Next { get; }

        public MainPageMiddleware(IBasePathProvider basePathProvider, IAuthorizationService authorizationService, IUIAuthorizationPolicyProvider uiAuthorizationPolicyProvider, IMainPageGenerator mainPageGenerator, RequestDelegate next)
        {
            AuthorizationService = authorizationService;
            UIAuthorizationPolicyProvider = uiAuthorizationPolicyProvider;
            MainPageGenerator = mainPageGenerator;
            Prefix1 = $"/{basePathProvider.BasePath}/";
            Prefix2 = $"/{basePathProvider.BasePath}";
            Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.ToString();

            if (!path.Equals(Prefix1, StringComparison.InvariantCultureIgnoreCase) && !path.Equals(Prefix2, StringComparison.InvariantCultureIgnoreCase))
            {
                await Next(httpContext);
                return;
            }

            if (UIAuthorizationPolicyProvider.AuthorizationPolicy != null)
            {
                if (!(await AuthorizationService.AuthorizeAsync(httpContext.User, null, UIAuthorizationPolicyProvider.AuthorizationPolicy)).Succeeded)
                {
                    foreach (string authenticationScheme in UIAuthorizationPolicyProvider.AuthorizationPolicy.AuthenticationSchemes)
                    {
                        await httpContext.ForbidAsync(authenticationScheme);
                        return;
                    }

                    await httpContext.ForbidAsync();
                    return;
                }
            }

            var mediaType = new MediaTypeHeaderValue("text/html");
            mediaType.CharSet = Encoding.UTF8.WebName;
            httpContext.Response.ContentType = mediaType.ToString();

            MainPageGenerator.Generate(httpContext.Response.Body);
        }
    }
}
