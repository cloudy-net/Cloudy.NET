using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class PrimaryKeyGetter : IPrimaryKeyGetter
    {
        IDictionary<Type, IEnumerable<PropertyInfo>> CachedKeys { get; } = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        IContextProvider ContextProvider { get; }

        public PrimaryKeyGetter(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public object[] Get(object content)
        {
            var type = content.GetType();

            if (!CachedKeys.ContainsKey(type))
            {
                var context = ContextProvider.GetFor(type);
                CachedKeys[type] = context.Context.Model.FindEntityType(type).FindPrimaryKey().Properties.Select(p => p.PropertyInfo).ToList().AsReadOnly();
            }

            return CachedKeys[type].Select(p => p.GetValue(content)).ToArray();
        }
    }
}