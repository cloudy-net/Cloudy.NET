using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMatchCreator
    {
        UIHintControlMatch Create(UIHint uiHint, UIHintControlMapping mapping);
    }
}