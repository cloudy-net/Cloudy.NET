using Cloudy.CMS.UI;
using Cloudy.CMS.UI.IdentitySupport;
using Cloudy.CMS.UI.PortalSupport;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI
{
    public class RequestPipelineBuilder : IRequestPipelineBuilder
    {
        public void Build(IApplicationBuilder app, CloudyAdminOptions options)
        {
            if (options.StaticFileProvider != null)
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = "/files",
                    FileProvider = options.StaticFileProvider,
                    OnPrepareResponse = context => context.Context.Response.Headers["Cache-Control"] = "no-cache"
                });
            }
            else if(options.StaticFilesBasePath != null)
            {

                app.Use(async (context, next) =>
                {
                    if (!context.Request.Path.Value.StartsWith("/files/")) {
                        await next.Invoke();
                        return;
                    }

                    context.Response.StatusCode = 302;
                    context.Response.Headers[HeaderNames.Location] = options.StaticFilesBasePath + context.Request.Path.Value.Substring("/files".Length);
                });
            }

            app.UseRouting();
            app.UseAuthentication();
            
            if (options.AuthorizeOptions != null)
            {
                if (app.ApplicationServices.GetService<IAuthorizationService>() == null)
                {
                    throw new Exception($"Could not find {nameof(IAuthorizationService)} in DI container. Call services.{nameof(PolicyServiceCollectionExtensions.AddAuthorization)}() in ConfigureServices");
                }

                app.Map(new PathString("/Login"), branch => app.ApplicationServices.GetService<ILoginPipelineBuilder>().Build(branch, options));
                
                var policy =
                    options.AuthorizeOptions != null ?
                    AuthorizationPolicy.CombineAsync(app.ApplicationServices.GetRequiredService<IAuthorizationPolicyProvider>(), new List<IAuthorizeData> { options.AuthorizeOptions }).Result :
                    new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                app.UseMiddleware<AuthorizeMiddleware>(policy);
            }
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    if (!PathEndsInSlash(context.Request.Path))
                    {
                        RedirectToPathWithSlash(context);
                        return;
                    }

                    await context.RequestServices.GetRequiredService<IPortalPageRenderer>().RenderPageAsync(context);
                });
                endpoints.MapAreaControllerRoute(null, "Cloudy.CMS", "{controller}/{action}");
            });
        }

        bool PathEndsInSlash(PathString path)
        {
            return path.Value.EndsWith("/", StringComparison.Ordinal);
        }

        void RedirectToPathWithSlash(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
            var request = context.Request;
            var redirect = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path + "/", request.QueryString);
            context.Response.Headers[HeaderNames.Location] = redirect;
        }
    }
}