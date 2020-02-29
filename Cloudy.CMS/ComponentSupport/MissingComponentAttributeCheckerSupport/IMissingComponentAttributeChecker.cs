using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport
{
    public interface IMissingComponentAttributeChecker
    {
        void Check(IEnumerable<Type> types);
    }
}