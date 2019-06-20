using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Id}({Parameters.Count})")]
    public class UIHint
    {
        public string Id { get; }
        public IList<UIHintParameterValue> Parameters { get; }

        public UIHint(string id, IEnumerable<UIHintParameterValue> parameters)
        {
            Id = id;
            Parameters = parameters.ToList().AsReadOnly();
        }
    }
}
