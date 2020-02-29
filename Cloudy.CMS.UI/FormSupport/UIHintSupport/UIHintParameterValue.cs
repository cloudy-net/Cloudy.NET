using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Type}")]
    public class UIHintParameterValue
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UIHintParameterType Type { get; }
        public object Value { get; }
        public UIHintParameterValue(string value)
        {
            Type = UIHintParameterType.String;
            Value = value;
        }

        public UIHintParameterValue(int value)
        {
            Type = UIHintParameterType.Number;
            Value = value;
        }

        public UIHintParameterValue(Expression value)
        {
            Type = UIHintParameterType.Expression;
            Value = value;
        }

        public UIHintParameterValue(IDictionary<string, UIHintParameterValue> value)
        {
            Type = UIHintParameterType.Object;
            Value = value;
        }
    }
}