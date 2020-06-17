using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.DocumentSupport.CacheSupport;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.DocumentSupport.InMemorySupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddInMemory(this CloudyConfigurator instance)
        {
            instance.AddCachedDocuments();
            instance.Services.AddSingleton<IDataSource, InMemoryDataSource>();
            return instance;
        }
    }
}
