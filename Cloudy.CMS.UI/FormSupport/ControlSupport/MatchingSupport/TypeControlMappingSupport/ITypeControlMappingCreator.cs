using Cloudy.CMS.ComposableSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public interface ITypeControlMappingCreator : IComposable
    {
        IEnumerable<TypeControlMapping> Create();
    }
}