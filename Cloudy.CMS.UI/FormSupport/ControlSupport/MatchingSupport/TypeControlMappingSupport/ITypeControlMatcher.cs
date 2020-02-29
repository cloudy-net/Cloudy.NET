using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public interface ITypeControlMatcher
    {
        TypeControlMatch GetFor(Type type);
    }
}