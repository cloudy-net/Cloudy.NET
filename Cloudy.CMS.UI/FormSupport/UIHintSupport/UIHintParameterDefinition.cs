using System.Diagnostics;

namespace Poetry.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Id}: {Type}")]
    public class UIHintParameterDefinition
    {
        public string Id { get; }
        public UIHintParameterType Type { get; }

        public UIHintParameterDefinition(string id, UIHintParameterType type)
        {
            Id = id;
            Type = type;
        }
    }
}