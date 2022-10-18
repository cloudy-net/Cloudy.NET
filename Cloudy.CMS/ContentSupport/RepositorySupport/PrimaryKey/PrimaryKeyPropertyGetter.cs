using Cloudy.CMS.ContextSupport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey
{
    public class PrimaryKeyPropertyGetter : IPrimaryKeyPropertyGetter
    {
        IDictionary<Type, IEnumerable<PropertyInfo>> CachedKeys { get; } = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        IContextProvider ContextProvider { get; }

        public PrimaryKeyPropertyGetter(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public IEnumerable<PropertyInfo> GetFor(Type type)
        {
            if (!CachedKeys.ContainsKey(type))
            {
                var context = ContextProvider.GetFor(type);
                CachedKeys[type] = context.Context.Model.FindEntityType(type).FindPrimaryKey().Properties.Select(p => p.PropertyInfo).ToList().AsReadOnly();
            }

            return CachedKeys[type];
        }
    }
}
