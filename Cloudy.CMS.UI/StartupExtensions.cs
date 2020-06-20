using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS;
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
using Microsoft.AspNetCore.Routing;
using Cloudy.CMS.AspNetCore;
using Cloudy.CMS.UI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.InitializerSupport;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.UI.PortalSupport;
using Cloudy.CMS.UI;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Builder
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddAdmin(this CloudyConfigurator configurator)
        {
            configurator.AddComponent<CloudyAdminComponent>();
            configurator.Services.AddSingleton(new CloudyAdminOptions());

            return configurator;
        }

        public static void UseCloudyAdmin(this IApplicationBuilder app, Action<CloudyAdminConfigurator> configure)
        {
            if (app.ApplicationServices.GetService(typeof(IComponentTypeProvider)) == null)
            {
                throw new Exception("Please add Cloudy services first by doing: services.AddCloudy(...)");
            }

            if (!((IComponentTypeProvider)app.ApplicationServices.GetService(typeof(IComponentTypeProvider))).GetAll().Contains(typeof(CloudyAdminComponent)))
            {
                throw new Exception("Please add Cloudy Admin services first by doing: services.AddCloudy(cloudy => cloudy.AddAdmin())");
            }

            var options = app.ApplicationServices.GetService<CloudyAdminOptions>();
            var configurator = new CloudyAdminConfigurator(options);

            configure(configurator);

            if (options.StaticFilesBasePath == null && options.StaticFileProvider == null)
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;

                if (version.Equals(new Version(1, 0, 0, 0)))
                {
                    throw new Exception("It seems you have linked Cloudy CMS through a project reference. Please add something like configure.WithStaticFilesFrom(new PhysicalFileProvider(Path.Combine(env.ContentRootPath, \"../cloudy-cms/Cloudy.CMS.UI/wwwroot\"))) to find your local static files");
                }

                configurator.WithStaticFilesFromVersion(version);
            }

            app.Map(new PathString(options.BasePath), branch => app.ApplicationServices.GetService<IRequestPipelineBuilder>().Build(branch, options));
        }
    }
}