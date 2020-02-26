using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Poetry.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport
{
    public interface IControlMatcher
    {
        IControlMatch GetFor(Type type, IEnumerable<UIHint> uiHints);
    }
}
