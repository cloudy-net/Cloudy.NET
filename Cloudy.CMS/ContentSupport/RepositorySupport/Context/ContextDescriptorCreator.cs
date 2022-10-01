using Cloudy.CMS.ContentSupport.RepositorySupport.DbSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
{
    public class ContextDescriptorCreator : IContextDescriptorCreator
    {
        public IEnumerable<ContextDescriptor> Create(IEnumerable<Type> types)
        {
            var result = new List<ContextDescriptor>();

            foreach(var type in types)
            {
                var dbSets = type.GetProperties()
                    .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                    .Select(p => new DbSetDescriptor(p.PropertyType.GetGenericArguments()[0], p));

                result.Add(new ContextDescriptor(type, dbSets));
            }

            return result;
        }
    }
}
