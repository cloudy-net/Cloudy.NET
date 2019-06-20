using Poetry.ComposableSupport;
using System.Collections.Generic;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMappingCreator : IComposable
    {
        IEnumerable<UIHintControlMapping> Create();
    }
}