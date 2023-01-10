using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.EntityTypeSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.BlockSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IBlockTypeCreator, BlockTypeCreator>();
            services.AddSingleton<IBlockTypeProvider, BlockTypeProvider>();
        }
    }
}
