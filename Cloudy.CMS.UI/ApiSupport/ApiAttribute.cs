using System;
using System.Collections.Generic;

namespace Poetry.UI.ApiSupport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiAttribute : Attribute
    {
        public string Id { get; }

        public ApiAttribute(string id)
        {
            Id = id;
        }
    }
}
