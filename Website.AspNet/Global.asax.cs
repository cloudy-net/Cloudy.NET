using Microsoft.Extensions.Logging;
using Poetry.AspNet;
using Cloudy.CMS.AspNet;
using Cloudy.CMS.UI;
using Poetry.UI;
using Poetry.UI.AspNet;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Website.AspNet
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Server.MapPath("~/Logs/log-{Date}.txt"))
                .CreateLogger();

            this.AddPoetry(c =>
            {
                c.AddLogging(l => l.AddSerilog());
                c.AddUI();
                c.AddCMS();
                c.AddCMSUI();
                c.AddComponent<WebsiteComponent>();
            });

            RouteTable.Routes.AddContentRoute();
        }
    }
}
