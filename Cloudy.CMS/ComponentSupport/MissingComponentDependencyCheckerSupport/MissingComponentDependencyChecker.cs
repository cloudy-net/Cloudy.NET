using Poetry.ComponentSupport.DependencySupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.ComponentSupport.MissingComponentDependencyCheckerSupport
{
    public class MissingComponentDependencyChecker : IMissingComponentDependencyChecker
    {
        public void Check(IEnumerable<Type> types)
        {
            var ids = new HashSet<string>(types.Select(t => t.GetCustomAttribute<ComponentAttribute>().Id));

            foreach(var type in types)
            {
                foreach(var dependency in type.GetCustomAttributes<DependencyAttribute>())
                {
                    if (!ids.Contains(dependency.Id))
                    {
                        throw new MissingComponentDependencyException(type.GetCustomAttribute<ComponentAttribute>().Id, dependency.Id);
                    }
                }
            }
        }
    }
}
