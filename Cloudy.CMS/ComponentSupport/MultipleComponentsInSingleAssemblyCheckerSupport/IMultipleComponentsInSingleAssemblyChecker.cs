using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport
{
    public interface IMultipleComponentsInSingleAssemblyChecker
    {
        void Check(IEnumerable<Type> componentTypes);
    }
}
