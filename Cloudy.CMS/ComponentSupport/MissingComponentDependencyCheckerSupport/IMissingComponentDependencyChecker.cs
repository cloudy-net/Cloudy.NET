using System;
using System.Collections.Generic;

namespace Poetry.ComponentSupport.MissingComponentDependencyCheckerSupport
{
    public interface IMissingComponentDependencyChecker
    {
        void Check(IEnumerable<Type> types);
    }
}