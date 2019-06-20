using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace Poetry.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Value} ({Type})")]
    public class ExpressionSegment
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ExpressionSegmentType Type { get; }
        public string Value { get; }

        public ExpressionSegment(ExpressionSegmentType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}