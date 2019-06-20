using Poetry.UI.ApiSupport;
using Poetry.UI.AppSupport;
using Poetry.UI.PortalSupport;
using System;
using System.Collections.Generic;
using Poetry.DependencyInjectionSupport;
using Poetry.ComponentSupport;
using Poetry.EmbeddedResourceSupport;

namespace Poetry.UI
{
    public class PoetryUIConfigurator
    {
        string BasePath { get; set; } = "Admin";
        PoetryConfigurator PoetryConfigurator { get; }

        public PoetryUIConfigurator(PoetryConfigurator poetryConfigurator)
        {
            PoetryConfigurator = poetryConfigurator;
        }

        public PoetryUIConfigurator WithBasePath(string basePath)
        {
            BasePath = basePath.Trim('/');
            return this;
        }

        public void Done()
        {
            PoetryConfigurator.InjectSingleton<IBasePathProvider>(new BasePathProvider(BasePath));
        }
    }
}
