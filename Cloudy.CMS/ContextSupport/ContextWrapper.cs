using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.ContextSupport
{
    public class ContextWrapper : IContextWrapper
    {
        public DbContext Context { get; }

        IDictionary<Type, PropertyInfo> DbSetsByType { get; }

        public ContextWrapper(DbContext context)
        {
            Context = context;
            DbSetsByType = context.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToDictionary(p => p.PropertyType.GetGenericArguments()[0], p => p);
        }

        public object GetDbSet(Type type)
        {
            if (DbSetsByType.ContainsKey(type))
            {
                return DbSetsByType[type].GetValue(Context);
            }

            foreach (var pair in DbSetsByType)
            {
                if (pair.Key.IsAssignableFrom(type))
                {
                    return pair.Value.GetValue(Context);
                }
            }

            return null;
        }
    }
}