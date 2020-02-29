using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport
{
    public class MissingComponentAttributeChecker : IMissingComponentAttributeChecker
    {
        public void Check(IEnumerable<Type> types)
        {
            foreach(var type in types)
            {
                if(type.GetCustomAttribute<ComponentAttribute>() == null)
                {
                    throw new MissingComponentAttributeException(type);
                }
            }
        }
    }
}
