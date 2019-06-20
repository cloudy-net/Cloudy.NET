using Poetry.ComposableSupport;
using System.Collections.Generic;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public interface ITypeControlMappingCreator : IComposable
    {
        IEnumerable<TypeControlMapping> Create();
    }
}