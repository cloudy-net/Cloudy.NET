using Microsoft.Extensions.DependencyInjection;
using Poetry;
using Cloudy.CMS;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cloudy.CMS.UI.NaggingSupport;
using Microsoft.AspNetCore.Routing;
using Poetry.AspNetCore;
using Poetry.UI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Poetry.InitializerSupport;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Poetry.ComponentSupport;
using Cloudy.CMS.UI.AuthorizationSupport;
using Cloudy.CMS.UI.PortalSupport;
using Cloudy.CMS.UI;

namespace Cloudy.CMS
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddAdmin(this CloudyConfigurator configurator)
        {
            configurator.AddComponent<CloudyAdminComponent>();

            return configurator;
        }

        public static void UseCloudyAdmin(this IApplicationBuilder app, Action<CloudyAdminConfigurator> configure)
        {
            var options = new CloudyAdminOptions();

            configure(new CloudyAdminConfigurator(options));

            if (!options.AllowUnauthenticatedUsers && options.AuthorizeOptions == null)
            {
                throw new ArgumentException($"You have called neither {nameof(CloudyAdminConfigurator.Authorize)}() or {nameof(CloudyAdminConfigurator.Unprotect)}(). You probably want to use the first one");
            }

            if (options.AllowUnauthenticatedUsers && options.AuthorizeOptions != null)
            {
                throw new ArgumentException($"You have called both {nameof(CloudyAdminConfigurator.Authorize)}() and {nameof(CloudyAdminConfigurator.Unprotect)}(), they are mutually exclusive. You probably want to remove the latter");
            }

            var authorizationService = app.ApplicationServices.GetService<IAuthorizationService>();

            if(authorizationService == null)
            {
                throw new Exception($"Could not find {nameof(IAuthorizationService)} in DI container. Call services.{nameof(AuthenticationServiceCollectionExtensions.AddAuthentication)}() in ConfigureServices");
            }

            var policy = 
                options.AllowUnauthenticatedUsers ?
                new AuthorizationPolicyBuilder().RequireAssertion(context => true).Build() :
                options.AuthorizeOptions != null ?
                AuthorizationPolicy.CombineAsync(app.ApplicationServices.GetRequiredService<IAuthorizationPolicyProvider>(), new List<IAuthorizeData> { options.AuthorizeOptions }).Result :
                new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            var path = new PathString(options.BasePath);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            ((StaticFilesBasePathProvider)app.ApplicationServices.GetRequiredService<IStaticFilesBasePathProvider>()).StaticFilesBasePath = $"https://cloudycmsui.blob.core.windows.net/v-{version.Major}-{version.Minor}-{version.Build}";

            app.Map(path, adminBranch =>
            {
                adminBranch.UseMiddleware<AuthorizeMiddleware>(policy);

                if (options.StaticFilesFileProvider != null)
                {
                    adminBranch.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = options.StaticFilesFileProvider,
                        OnPrepareResponse = context => context.Context.Response.Headers["Cache-Control"] = "no-cache"
                    });
                }

                if (options.StaticFilesBaseUri != null)
                {
                    ((StaticFilesBasePathProvider)app.ApplicationServices.GetRequiredService<IStaticFilesBasePathProvider>()).StaticFilesBasePath = options.StaticFilesBaseUri;
                }

                adminBranch.UseRouting();
                adminBranch.UseEndpoints(endpoints => {
                    endpoints.MapGet("/", async context => await context.RequestServices.GetRequiredService<IPortalPageRenderer>().RenderPageAsync(context));
                    endpoints.MapAreaControllerRoute(null, "Cloudy.CMS", "{controller}/{action}");
                });
            });
        }
    }
}