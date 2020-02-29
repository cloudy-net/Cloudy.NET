using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport
{
    public class ComponentTypeProvider : IComponentTypeProvider
    {
        IEnumerable<Type> Types { get; }

        public ComponentTypeProvider(IEnumerable<Type> types)
        {
            Types = types.ToList().AsReadOnly();
        }

        public IEnumerable<Type> GetAll()
        {
            return Types;
        }
    }
}
