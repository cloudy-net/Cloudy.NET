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
        RequestDelegate Next { get; }

        public MainPageMiddleware(IAuthorizationService authorizationService, IUIAuthorizationPolicyProvider uiAuthorizationPolicyProvider, IMainPageGenerator mainPageGenerator, RequestDelegate next)
        {
            AuthorizationService = authorizationService;
            UIAuthorizationPolicyProvider = uiAuthorizationPolicyProvider;
            MainPageGenerator = mainPageGenerator;
            Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
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
