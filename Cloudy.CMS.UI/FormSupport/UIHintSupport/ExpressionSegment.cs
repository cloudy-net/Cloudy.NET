using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport
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