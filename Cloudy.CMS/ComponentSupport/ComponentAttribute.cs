using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute {
        public string Id { get; }

        public ComponentAttribute(string id)
        {
            Id = id;
        }
    }
}
