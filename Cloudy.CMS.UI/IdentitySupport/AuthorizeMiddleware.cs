using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
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

            if (context.Request.QueryString.Value.Contains("ReturnUrl%253D"))
            {
                throw new Exception("Redirection loop detected during authorization. Did you UseAuthentication?");
            }

            await context.ChallengeAsync(IdentityConstants.ApplicationScheme, new AuthenticationProperties
            {
                RedirectUri = context.Request.PathBase.Add(context.Request.Path).Value + context.Request.QueryString,
            });
        }
    }
}
