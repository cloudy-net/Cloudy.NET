using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AuthorizationSupport
{
    public class AuthorizeMiddleware
    {
        RequestDelegate Next { get; }
        AuthorizationPolicy Policy { get; }
        IAuthorizationService AuthorizationService { get; }

        public AuthorizeMiddleware(AuthorizationPolicy policy, RequestDelegate next, IAuthorizationService authorizationService)
        {
            Next = next;
            Policy = policy;
            AuthorizationService = authorizationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if ((await AuthorizationService.AuthorizeAsync(context.User, Policy)).Succeeded)
            {
                await Next(context);
                return;
            }

            if (context.User.Identity.IsAuthenticated)
            {
                await context.ForbidAsync();
                return;
            }

            await context.ChallengeAsync(new AuthenticationProperties()
            {
                RedirectUri = context.Request.PathBase + context.Request.Path + context.Request.QueryString,
            });
        }
    }
}
