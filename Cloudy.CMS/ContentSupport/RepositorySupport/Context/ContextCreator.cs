using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
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
            var contextDescriptor = ContextDescriptorProvider.GetFor(type);

            if (contextDescriptor == null)
            {
                throw new CouldNotFindAnyDbContextWithDbSetForTypeException(type);
            }

            return new ContextWrapper((DbContext)ServiceProvider.GetRequiredService(contextDescriptor.Type));
        }
    }
}
