using Cloudy.CMS.ContextSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.EntitySupport.PrimaryKey
{
    public class PrimaryKeySetter : IPrimaryKeySetter
    {
        IDictionary<Type, IEnumerable<PropertyInfo>> CachedKeys { get; } = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        IContextProvider ContextProvider { get; }

        public PrimaryKeySetter(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public void Set(IEnumerable<object> keyValues, object content)
        {
            var type = content.GetType();

            if (!CachedKeys.ContainsKey(type))
            {
                var context = ContextProvider.GetFor(content.GetType());
                CachedKeys[type] = context.Context.Model.FindEntityType(content.GetType()).FindPrimaryKey().Properties.Select(p => p.PropertyInfo).ToList().AsReadOnly();
            }

            for (var i = 0; i < CachedKeys[type].Count(); i++)
            {
                CachedKeys[type].ElementAt(i).SetValue(content, keyValues.ElementAt(i));
            }
        }
    }
}
