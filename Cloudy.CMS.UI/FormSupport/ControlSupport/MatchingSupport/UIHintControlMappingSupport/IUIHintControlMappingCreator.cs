using Cloudy.CMS.ComposableSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public interface IUIHintControlMappingCreator : IComposable
    {
        IEnumerable<UIHintControlMapping> Create();
    }
}