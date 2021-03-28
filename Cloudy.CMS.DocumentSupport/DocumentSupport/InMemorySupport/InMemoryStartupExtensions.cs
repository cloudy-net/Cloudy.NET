using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS;
using Cloudy.CMS.DocumentSupport.CacheSupport;
using Cloudy.CMS.DocumentSupport.InMemorySupport;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class InMemoryStartupExtensions
    {
        public static CloudyConfigurator AddInMemoryDocuments(this CloudyConfigurator cloudy)
        {
            cloudy.AddCachedDocuments<InMemoryDataSource>();
            return cloudy;
        }
    }
}
