using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMappingCreator
    {
        IEnumerable<UIHintControlMapping> Create();
    }
}