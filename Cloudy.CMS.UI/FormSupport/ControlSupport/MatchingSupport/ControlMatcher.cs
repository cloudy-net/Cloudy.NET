using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport
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

        public IControlMatch GetFor(Type type, IEnumerable<UIHint> uiHints)
        {
            foreach (var uiHint in uiHints)
            {
                var match = UIHintControlMatcher.GetFor(uiHint);

                if(match != null)
                {
                    return match;
                }
            }

            {
                var match = TypeControlMatcher.GetFor(type);

                if (match != null)
                {
                    return match;
                }
            }

            foreach (var uiHint in uiHints)
            {
                if (uiHint.Parameters.Any())
                {
                    continue;
                }

                if (Controls.ContainsKey(uiHint.Id)) {
                    return new UIHintControlMatch(uiHint.Id, uiHint.Id, new Dictionary<string, object>());
                }
            }

            return null;
        }
    }
}
