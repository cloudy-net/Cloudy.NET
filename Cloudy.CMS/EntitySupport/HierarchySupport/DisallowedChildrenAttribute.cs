using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport.HierarchySupport
{
    public class DisallowedChildrenAttribute : Attribute
    {
        public IEnumerable<Type> Types { get; set; }

        public DisallowedChildrenAttribute(params Type[] types)
        {
            Types = types;
        }
    }

    public class DisallowedChildrenAttribute<T1> : DisallowedChildrenAttribute
    {
        public DisallowedChildrenAttribute() : base(new Type[] { typeof(T1) }) { }
    }

    public class DisallowedChildrenAttribute<T1, T2> : DisallowedChildrenAttribute
    {
        public DisallowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2) }) { }
    }

    public class DisallowedChildrenAttribute<T1, T2, T3> : DisallowedChildrenAttribute
    {
        public DisallowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2), typeof(T3) }) { }
    }

    public class DisallowedChildrenAttribute<T1, T2, T3, T4> : DisallowedChildrenAttribute
    {
        public DisallowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }) { }
    }

    public class DisallowedChildrenAttribute<T1, T2, T3, T4, T5> : DisallowedChildrenAttribute
    {
        public DisallowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }) { }
    }
}
