using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMatcher
    {
        UIHintControlMatch GetFor(UIHint uiHint);
    }
}