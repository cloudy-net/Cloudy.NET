using Cloudy.CMS.UI.FormSupport.ControlSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMatch : IControlMatch
    {
        public string Id { get; }
        public string UIHint { get; }
        public IDictionary<string, object> Parameters { get; }

        public UIHintControlMatch(string id, string uiHint, IDictionary<string, object> parameters)
        {
            Id = id;
            UIHint = uiHint;
            Parameters = new ReadOnlyDictionary<string, object>(parameters);
        }
    }
}
