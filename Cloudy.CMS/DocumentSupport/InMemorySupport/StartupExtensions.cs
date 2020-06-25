using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.DocumentSupport.CacheSupport;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.DocumentSupport.InMemorySupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddInMemoryDocuments(this CloudyConfigurator cloudy)
        {
            cloudy.AddCachedDocuments<InMemoryDataSource>();
            return cloudy;
        }
    }
}
