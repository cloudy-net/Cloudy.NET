using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport
{
    public interface IPolymorphicFormFinder
    {
        IEnumerable<string> FindFor(Type type);
    }
}