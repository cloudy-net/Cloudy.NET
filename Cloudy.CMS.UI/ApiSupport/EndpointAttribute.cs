using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndpointAttribute : Attribute
    {
        public string Id { get; }
        
        public EndpointAttribute(string id)
        {
            Id = id;
        }
    }
}
