using Poetry.UI.AspNetCore.EmbeddedResourceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poetry.UI.AspNetCore
{
    public static class PoetryUIConfiguratorExtensions
    {
        public static PoetryUIConfigurator DevelopmentMode(this PoetryUIConfigurator instance, params string[] hostnames)
        {
            EmbeddedResourcesMiddleware.HostnamesActivatedForDevelopmentMode = hostnames.ToList().AsReadOnly();

            return instance;
        }
    }
}
