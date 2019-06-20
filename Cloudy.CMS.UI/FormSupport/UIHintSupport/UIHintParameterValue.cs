using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poetry.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Type}")]
    public class UIHintParameterValue
    {
        [JsonConverter(typeof(StringEnumConverter))]
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