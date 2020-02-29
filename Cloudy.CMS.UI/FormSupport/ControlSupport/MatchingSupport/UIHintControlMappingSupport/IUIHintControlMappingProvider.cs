using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMappingProvider
    {
        IEnumerable<UIHintControlMapping> GetFor(string uiHintId);
    }
}
