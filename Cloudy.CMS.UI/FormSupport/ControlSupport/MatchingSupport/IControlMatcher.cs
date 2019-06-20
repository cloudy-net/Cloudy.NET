using Poetry.UI.FormSupport.UIHintSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport
{
    public interface IControlMatcher
    {
        ControlReference GetFor(Type type, IEnumerable<UIHint> uiHints);
    }
}
