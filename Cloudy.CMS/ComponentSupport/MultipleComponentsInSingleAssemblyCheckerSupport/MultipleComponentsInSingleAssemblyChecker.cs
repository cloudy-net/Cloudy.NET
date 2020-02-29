using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport
{
    public class MultipleComponentsInSingleAssemblyChecker : IMultipleComponentsInSingleAssemblyChecker
    {
        public void Check(IEnumerable<Type> componentTypes)
        {
            var assembliesWithMultipleComponents = componentTypes.GroupBy(t => t.Assembly).Where(g => g.Count() > 1);

            if (assembliesWithMultipleComponents.Any())
            {
                var assemblyWithMultipleComponents = assembliesWithMultipleComponents.First();

                throw new MultipleComponentsInSingleAssemblyException(assemblyWithMultipleComponents.Key, assemblyWithMultipleComponents);
            }
        }
    }
}
