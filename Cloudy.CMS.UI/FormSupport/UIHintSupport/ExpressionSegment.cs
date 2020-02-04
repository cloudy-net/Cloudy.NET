using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Poetry.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Value} ({Type})")]
    public class ExpressionSegment
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExpressionSegmentType Type { get; }
        public string Value { get; }

        public ExpressionSegment(ExpressionSegmentType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}