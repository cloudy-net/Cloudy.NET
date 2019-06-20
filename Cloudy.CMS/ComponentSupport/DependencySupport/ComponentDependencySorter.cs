using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.ComponentSupport.DependencySupport
{
    public class ComponentDependencySorter : IComponentDependencySorter
    {
        public IEnumerable<ComponentDescriptor> Sort(IEnumerable<ComponentDescriptor> components)
        {
            var result = new List<ComponentDescriptor>();

            foreach(var component in components)
            {
                var index = result.Count;

                var dependents = components.Where(c => c.Dependencies.Contains(component.Id)).Where(c => result.Contains(c));

                if (dependents.Any())
                {
                    index = dependents.Select(d => result.IndexOf(d)).Min();
                }

                result.Insert(index, component);
            }

            return result;
        }
    }
}
