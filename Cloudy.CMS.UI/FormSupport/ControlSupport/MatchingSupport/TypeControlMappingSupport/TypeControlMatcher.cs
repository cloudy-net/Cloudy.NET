using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public class TypeControlMatcher : ITypeControlMatcher
    {
        IDictionary<Type, string> Mappings { get; }

        public TypeControlMatcher(IComposableProvider composableProvider)
        {
            Mappings = composableProvider
                .GetAll<ITypeControlMappingCreator>()
                .SelectMany(c => c.Create())
                .GroupBy(m => m.Type)
                .ToDictionary(m => m.Key, m => m.First().ControlId);
        }

        public TypeControlMatch GetFor(Type type)
        {
            if (!Mappings.ContainsKey(type))
            {
                return null;
            }

            return new TypeControlMatch(Mappings[type], type.Name);
        }
    }
}
