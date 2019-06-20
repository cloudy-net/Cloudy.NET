using Poetry.UI.FormSupport.UIHintSupport;
using System.Collections.Generic;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMatcher
    {
        ControlReference GetFor(UIHint uiHint);
    }
}