using System;
using System.Collections.Generic;
using System.Reflection;

namespace Poetry.ComponentSupport.DependencySupport
{
    public class ComponentDependencyCreator : IComponentDependencyCreator
    {
        public IEnumerable<string> Create(Type type)
        {
            var result = new List<string>();

            foreach(var attribute in type.GetCustomAttributes<DependencyAttribute>())
            {
                result.Add(attribute.Id);
            }

            return result;
        }
    }
}
