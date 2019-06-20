using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport
{
    public interface IMultipleComponentsInSingleAssemblyChecker
    {
        void Check(IEnumerable<Type> componentTypes);
    }
}
