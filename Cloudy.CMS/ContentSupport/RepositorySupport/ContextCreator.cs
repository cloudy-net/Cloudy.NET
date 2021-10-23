using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContextCreator : IContextCreator
    {
        IServiceProvider ServiceProvider { get; }
        IContextDescriptorProvider ContextDescriptorProvider { get; }

        public ContextCreator(IServiceProvider serviceProvider, IContextDescriptorProvider contextDescriptorProvider)
        {
            ServiceProvider = serviceProvider;
            ContextDescriptorProvider = contextDescriptorProvider;
        }

        public IContextWrapper CreateFor(Type type)
        {
            return new ContextWrapper((DbContext)ServiceProvider.GetRequiredService(ContextDescriptorProvider.GetFor(type).Type));
        }
    }
}
