using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport
{
    public class DuplicateComponentIdChecker : IDuplicateComponentIdChecker
    {
        public void Check(IEnumerable<Type> componentTypes)
        {
            var idsWithMultipleComponents = componentTypes.GroupBy(t => t.GetCustomAttribute<ComponentAttribute>().Id).Where(g => g.Count() > 1);

            if (idsWithMultipleComponents.Any())
            {
                var idWithMultipleComponents = idsWithMultipleComponents.First();

                throw new DuplicateComponentIdException(idWithMultipleComponents.Key, idWithMultipleComponents.Select(t => t.Assembly));
            }
        }
    }
}
