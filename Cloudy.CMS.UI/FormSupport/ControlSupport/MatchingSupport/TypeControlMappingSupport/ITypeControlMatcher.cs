using System;
using System.Collections.Generic;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public interface ITypeControlMatcher
    {
        ControlReference GetFor(Type type);
    }
}