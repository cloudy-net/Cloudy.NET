using System;
using System.Collections.Generic;
using System.Text;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Poetry.UI.FormSupport.UIHintSupport;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class ControlReferenceCreator : IControlReferenceCreator
    {
        public ControlReference Create(UIHint uiHint, UIHintControlMapping mapping)
        {
            var definition = mapping.UIHintDefinition;
            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < definition.Parameters.Count; i++)
            {
                parameters[definition.Parameters[i].Id] = uiHint.Parameters[i].Value;
            }

            return new ControlReference(mapping.ControlId, parameters);
        }
    }
}
