using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    /// <summary>
    /// Represents a Poetry.UI API endpoint.
    /// </summary>
    public class Endpoint
    {
        public string Id { get; }
        public MethodInfo Method { get; }

        public Endpoint(string id, MethodInfo method)
        {
            Id = id;
            Method = method;
        }
    }
}
