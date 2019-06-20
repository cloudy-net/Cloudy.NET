using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport.MissingComponentDependencyCheckerSupport
{
    public class MissingComponentDependencyException : Exception
    {
        public MissingComponentDependencyException(string componentId, string missingDependency) : base($"The component {componentId} depends on {missingDependency}, which was not found.") { }
    }
}
