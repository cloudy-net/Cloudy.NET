using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Poetry.AspNetCore;
using Cloudy.CMS.UI;
using Poetry.UI.AspNetCore;
using Cloudy.CMS.Mvc;
using Serilog;
using Website.AspNetCore.Models;
using Cloudy.CMS.SingletonSupport;
using Microsoft.Extensions.Configuration;
using Poetry.UI.AspNetCore.AuthorizationSupport;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Website.AspNetCore
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile("~/Logs/log-{Date}.txt")
                .CreateLogger();

            services.AddIdentityCore<User>().AddUserStore<UserRepository>();
            services
                .AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetSection("Authentication").GetSection("Google")["ClientId"];
                    options.ClientSecret = Configuration.GetSection("Authentication").GetSection("Google")["ClientSecret"];
                    options.CallbackPath = "/signin-google";
                    options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                    options.ClaimActions.Clear();
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                    options.ClaimActions.MapJsonKey("urn:google:profile", "link");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                });
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("admins", policy => policy.RequireAuthenticatedUser());
            });
            services.AddLogging(l => l.AddSerilog());
            services.AddPoetry(c =>
            {
                c.AddUI(/*ui => ui.SetAuthorizationPolicy(new UIAuthorizeOptions { Policy = "admins" })*/);
                c.AddCMS(cms => cms.SetDatabaseConnectionString("mongodb://localhost:27017/cms-web-test"));
                c.AddCMSUI(ui => ui.DontNagOnLocalhost());
                c.AddComponent<WebsiteComponent>();
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UsePoetry();
            app.UsePoetryUI();
            app.UseMvc(routeBuilder => routeBuilder
                .AddContentRoute()
            );
        }
    }
}
