using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    /// <summary>
    /// Represents a Poetry.UI API.
    /// 
    /// Note: Not to be inherited by your API classes as they should be POCOs, annotated with the [Api] attribute.
    /// </summary>
    public class Api
    {
        public string Id { get; }
        public Type Type { get; }
        public IEnumerable<Endpoint> Endpoints { get; }

        public Api(string id, Type type, IEnumerable<Endpoint> endpoints)
        {
            Id = id;
            Type = type;
            Endpoints = endpoints.ToList().AsReadOnly();
        }
    }
}
