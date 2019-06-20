using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using Poetry.UI.FormSupport.UIHintSupport;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IControlReferenceCreator
    {
        ControlReference Create(UIHint uiHint, UIHintControlMapping mapping);
    }
}