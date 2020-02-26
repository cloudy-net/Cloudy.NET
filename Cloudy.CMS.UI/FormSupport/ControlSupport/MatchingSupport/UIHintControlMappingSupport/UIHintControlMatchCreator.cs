using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Poetry.UI.FormSupport.UIHintSupport;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMatchCreator : IUIHintControlMatchCreator
    {
        public UIHintControlMatch Create(UIHint uiHint, UIHintControlMapping mapping)
        {
            var definition = mapping.UIHintDefinition;
            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < definition.Parameters.Count; i++)
            {
                parameters[definition.Parameters[i].Id] = uiHint.Parameters[i].Value;
            }

            return new UIHintControlMatch(mapping.ControlId, uiHint.Id, parameters);
        }
    }
}
