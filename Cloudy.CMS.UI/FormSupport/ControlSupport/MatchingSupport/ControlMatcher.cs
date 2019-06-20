using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Poetry.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport
{
    public class ControlMatcher : IControlMatcher
    {
        IUIHintControlMatcher UIHintControlMatcher { get; }
        ITypeControlMatcher TypeControlMatcher { get; }
        IDictionary<string, ControlDescriptor> Controls { get; }

        public ControlMatcher(IUIHintControlMatcher uiHintControlMatcher, ITypeControlMatcher typeControlMatcher, IControlProvider controlProvider)
        {
            TypeControlMatcher = typeControlMatcher;
            UIHintControlMatcher = uiHintControlMatcher;
            Controls = controlProvider.GetAll().ToDictionary(c => c.Id, c => c);
        }

        public ControlReference GetFor(Type type, IEnumerable<UIHint> uiHints)
        {
            foreach (var uiHint in uiHints)
            {
                var uiHintControlMatch = UIHintControlMatcher.GetFor(uiHint);

                if(uiHintControlMatch != null)
                {
                    return uiHintControlMatch;
                }
            }

            var typeControlMatch = TypeControlMatcher.GetFor(type);

            if(typeControlMatch != null)
            {
                return typeControlMatch;
            }

            foreach (var uiHint in uiHints)
            {
                if (uiHint.Parameters.Any())
                {
                    continue;
                }

                if (Controls.ContainsKey(uiHint.Id)) {
                    return new ControlReference(uiHint.Id, new Dictionary<string, object>());
                }
            }

            return null;
        }
    }
}
