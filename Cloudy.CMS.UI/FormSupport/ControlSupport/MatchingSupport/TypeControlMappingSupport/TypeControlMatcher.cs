using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public class TypeControlMatcher : ITypeControlMatcher
    {
        IDictionary<Type, string> Mappings { get; }

        public TypeControlMatcher(ITypeControlMappingCreator typeControlMappingCreator)
        {
            Mappings = typeControlMappingCreator.Create()
                .GroupBy(m => m.Type)
                .ToDictionary(m => m.Key, m => m.First().ControlId);
        }

        public TypeControlMatch GetFor(Type type)
        {
            if (!Mappings.ContainsKey(type))
            {
                return null;
            }

            return new TypeControlMatch(Mappings[type], type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? $"{type.GetGenericArguments()[0].Name}?" : type.Name);
        }
    }
}
