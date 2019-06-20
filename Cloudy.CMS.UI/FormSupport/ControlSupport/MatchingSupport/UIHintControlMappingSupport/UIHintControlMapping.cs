using Poetry.UI.FormSupport.UIHintSupport;
using System.Diagnostics;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    [DebuggerDisplay("{ControlId}={UIHintDefinition.Id}({UIHintDefinition.Parameters.Count})")]
    public class UIHintControlMapping
    {
        public UIHintDefinition UIHintDefinition { get; }
        public string ControlId { get; }

        public UIHintControlMapping(UIHintDefinition uiHintDefinition, string controlId)
        {
            UIHintDefinition = uiHintDefinition;
            ControlId = controlId;
        }
    }
}