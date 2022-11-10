using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public record DependencyInjector(IAssemblyProvider AssemblyProvider, IContextDescriptorProvider ContextDescriptorProvider) : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            foreach (var context in ContextDescriptorProvider.GetAll())
            {
                foreach (var dbSet in context.DbSets)
                {
                    if (!dbSet.Type.IsAssignableTo(typeof(ISingleton)))
                    {
                        continue;
                    }

                    services.AddTransient(dbSet.Type, serviceProvider => ((IQueryable)serviceProvider.GetService<IContextCreator>().CreateFor(context.Type).GetDbSet(dbSet.Type)).Cast<object>().FirstOrDefault());
                }
            }
        }
    }
}
