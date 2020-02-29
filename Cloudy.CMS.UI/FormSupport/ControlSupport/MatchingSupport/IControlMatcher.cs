using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport
{
    public interface IControlMatcher
    {
        IControlMatch GetFor(Type type, IEnumerable<UIHint> uiHints);
    }
}
