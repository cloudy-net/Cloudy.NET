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
        IUIHintControlMatchCreator ControlReferenceCreator { get; }
        IDictionary<string, IEnumerable<UIHintControlMapping>> Mappings { get; }

        public UIHintControlMatcher(IComposableProvider composableProvider, IUIHintControlMatchCreator controlReferenceCreator)
        {
            ControlReferenceCreator = controlReferenceCreator;
            Mappings = composableProvider
                .GetAll<IUIHintControlMappingCreator>()
                .SelectMany(c => c.Create())
                .GroupBy(m => m.UIHintDefinition.Id)
                .ToDictionary(m => m.Key, m => (IEnumerable<UIHintControlMapping>)m);
        }

        public UIHintControlMatch GetFor(UIHint uiHint)
        {
            if (!Mappings.ContainsKey(uiHint.Id))
            {
                return null;
            }

            var mapping = Mappings[uiHint.Id].Where(m => IsMatch(uiHint, m.UIHintDefinition)).FirstOrDefault();

            if (mapping != null)
            {
                return ControlReferenceCreator.Create(uiHint, mapping);
            }

            return null;
        }

        bool IsMatch(UIHint uiHint, UIHintDefinition definition)
        {
            if (definition.Parameters.Count != uiHint.Parameters.Count)
            {
                return false;
            }

            for (var i = 0; i < definition.Parameters.Count; i++)
            {
                var parameter = uiHint.Parameters[i];
                var parameterDefinition = definition.Parameters[i];

                if(parameterDefinition.Type == UIHintParameterType.Any)
                {
                    continue;
                }

                if (parameterDefinition.Type != parameter.Type)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
