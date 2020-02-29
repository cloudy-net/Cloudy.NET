using Cloudy.CMS.UI.FormSupport.UIHintSupport;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMatchEvaluator
    {
        bool IsMatch(UIHint uiHint, UIHintDefinition definition);
    }
}