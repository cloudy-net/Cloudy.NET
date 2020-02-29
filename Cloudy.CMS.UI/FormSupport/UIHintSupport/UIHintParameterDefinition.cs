using System.Diagnostics;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Id}: {Type}{(Optional ? \"?\" : \"\")}")]
    public class UIHintParameterDefinition
    {
        public string Id { get; }
        public UIHintParameterType Type { get; }
        public bool Optional { get; }

        public UIHintParameterDefinition(string id, UIHintParameterType type, bool optional)
        {
            Id = id;
            Type = type;
            Optional = optional;
        }
    }
}