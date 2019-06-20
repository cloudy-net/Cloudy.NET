using Poetry.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
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

        public ControlReference GetFor(Type type)
        {
            if (!Mappings.ContainsKey(type))
            {
                return null;
            }

            return new ControlReference(Mappings[type], new Dictionary<string, object>());
        }
    }
}
