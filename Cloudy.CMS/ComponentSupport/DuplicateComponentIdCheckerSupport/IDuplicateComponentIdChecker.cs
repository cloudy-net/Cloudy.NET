using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport.DuplicateComponentIdCheckerSupport
{
    public interface IDuplicateComponentIdChecker
    {
        void Check(IEnumerable<Type> componentTypes);
    }
}
