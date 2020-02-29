using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport
{
    public class MissingComponentAttributeException : Exception
    {
        public MissingComponentAttributeException(Type type) : base($"Type {type} needs to have the Component attribute to be registered as a component") { }
    }
}
