using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport
{
    public interface IDuplicateComponentIdChecker
    {
        void Check(IEnumerable<Type> componentTypes);
    }
}
