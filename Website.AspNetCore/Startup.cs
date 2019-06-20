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
using Cloudy.CMS;
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
            services.AddCloudy(cloudy => cloudy
                .WithDatabaseConnectionString("mongodb://localhost:27017/cms-web-test")
                .AddCloudyAdmin()
            );
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCloudyAdmin(cloudy => cloudy.WithBasePath("/Admin"));
            app.UseMvc(routeBuilder => routeBuilder.AddContentRoute());
        }
    }
}
