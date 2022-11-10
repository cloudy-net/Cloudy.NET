using Cloudy.CMS.ContextSupport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.EntitySupport.PrimaryKey
{
    public class PrimaryKeyPropertyGetter : IPrimaryKeyPropertyGetter
    {
        IDictionary<Type, IEnumerable<PropertyInfo>> CachedKeys { get; } = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        IContextCreator ContextCreator { get; }

        public PrimaryKeyPropertyGetter(IContextCreator contextCreator)
        {
            ContextCreator = contextCreator;
        }

        public IEnumerable<PropertyInfo> GetFor(Type type)
        {
            if (!CachedKeys.ContainsKey(type))
            {
                var context = ContextCreator.CreateFor(type);
                CachedKeys[type] = context.Context.Model.FindEntityType(type).FindPrimaryKey().Properties.Select(p => p.PropertyInfo).ToList().AsReadOnly();
            }

            return CachedKeys[type];
        }
    }
}
