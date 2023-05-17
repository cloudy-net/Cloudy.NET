using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.ContextSupport
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
