﻿using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IPropertyDefinitionCreator, PropertyDefinitionCreator>();
            services.AddSingleton<IPropertyMappingCreator, PropertyMappingCreator>();
            services.AddSingleton<IPropertyMappingProvider, PropertyMappingProvider>();

            services.AddSingleton<IContentTypeCreator, ContentTypeCreator>();
            services.AddSingleton<IContentTypeProvider, ContentTypeProvider>();
            services.AddSingleton<IPropertyDefinitionProvider, PropertyDefinitionProvider>();
        }
    }
}
