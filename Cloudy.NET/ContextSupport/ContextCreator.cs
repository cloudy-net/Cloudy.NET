using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cloudy.CMS.ContextSupport
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
                return null;
            }

            return new ContextWrapper((DbContext)ServiceProvider.GetRequiredService(contextDescriptor.Type));
        }
    }
}
