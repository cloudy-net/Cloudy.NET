using System;
using System.Collections.Generic;

namespace Poetry.ComponentSupport.MissingComponentAttributeCheckerSupport
{
    public interface IMissingComponentAttributeChecker
    {
        void Check(IEnumerable<Type> types);
    }
}