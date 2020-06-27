using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public static class StartupExtensions
    {
        public static IdentityBuilder AddCloudyIdentity(this IServiceCollection services) => services.AddCloudyIdentity<User>(_ => { });
        public static IdentityBuilder AddCloudyIdentity<TUser>(this IServiceCollection services, Action<IdentityOptions> configureOptions) where TUser : class
        {
            services.AddSingleton<INormalizer, Normalizer>();
            services.AddSingleton<ILoginPageBrandingPathProvider>(new LoginPageBrandingPathProvider(null));

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.Cookie.Path = "/";
                o.Events.OnRedirectToLogin = async context =>
                {
                    if(context.Request.Headers[HeaderNames.Accept] == "*/*")
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    context.Response.Redirect("/Admin/Login/");
                };
            });

            return services.AddIdentityCore<TUser>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                configureOptions?.Invoke(o);
            })
                .AddUserStore<UserStore>()
                .AddSignInManager()
                .AddUserManager<UserManager>()
                .AddDefaultTokenProviders();
        }
    }
}
