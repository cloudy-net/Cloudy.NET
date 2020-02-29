using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.ComposableSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMatcher : IUIHintControlMatcher
    {
        IUIHintControlMappingProvider UIHintControlMappingProvider { get; }
        IUIHintControlMatchEvaluator UIHintControlMatchEvaluator { get; }
        IUIHintControlMatchCreator ControlReferenceCreator { get; }

        public UIHintControlMatcher(IUIHintControlMappingProvider uiHintControlMappingProvider, IUIHintControlMatchEvaluator uiHintControlMatchEvaluator, IUIHintControlMatchCreator controlReferenceCreator)
        {
            UIHintControlMappingProvider = uiHintControlMappingProvider;
            UIHintControlMatchEvaluator = uiHintControlMatchEvaluator;
            ControlReferenceCreator = controlReferenceCreator;
        }

        public UIHintControlMatch GetFor(UIHint uiHint)
        {
            var mapping = UIHintControlMappingProvider
                .GetFor(uiHint.Id)
                .Where(m => UIHintControlMatchEvaluator.IsMatch(uiHint, m.UIHintDefinition))
                .FirstOrDefault();

            if (mapping == null)
            {
                return null;
            }

            return ControlReferenceCreator.Create(uiHint, mapping);
        }
    }
}
