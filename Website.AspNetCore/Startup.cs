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

namespace Website.AspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile("~/Logs/log-{Date}.txt")
                .CreateLogger();

            services.AddLogging(l => l.AddSerilog());
            services.AddPoetry(c =>
            {
                c.AddUI();
                c.AddCMS();
                c.AddCMSUI();
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
            app.UsePoetry();
            app.UsePoetryUI();
            app.UseMvc(routeBuilder => routeBuilder
                .AddContentRoute()
            );
        }
    }
}
