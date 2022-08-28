using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public interface ITypeControlMappingCreator
    {
        IEnumerable<TypeControlMapping> Create();
    }
}