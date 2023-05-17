using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.EntitySupport.HierarchySupport
{
    public class AllowedChildrenAttribute : Attribute
    {
        public IEnumerable<Type> Types { get; set; }

        public AllowedChildrenAttribute(params Type[] types)
        {
            Types = types;
        }
    }

    public class AllowedChildrenAttribute<T1> : AllowedChildrenAttribute
    {
        public AllowedChildrenAttribute() : base(new Type[] { typeof(T1) }) { }
    }

    public class AllowedChildrenAttribute<T1, T2> : AllowedChildrenAttribute
    {
        public AllowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2) }) { }
    }

    public class AllowedChildrenAttribute<T1, T2, T3> : AllowedChildrenAttribute
    {
        public AllowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2), typeof(T3) }) { }
    }

    public class AllowedChildrenAttribute<T1, T2, T3, T4> : AllowedChildrenAttribute
    {
        public AllowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }) { }
    }

    public class AllowedChildrenAttribute<T1, T2, T3, T4, T5> : AllowedChildrenAttribute
    {
        public AllowedChildrenAttribute() : base(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }) { }
    }
}
