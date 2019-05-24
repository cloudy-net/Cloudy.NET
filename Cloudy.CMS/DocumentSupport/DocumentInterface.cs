using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentInterface
    {
        public string Id { get; set; }
        public IDictionary<string, object> Properties { get; set; }

        public DocumentInterface(string id, IDictionary<string, object> properties)
        {
            Id = id;
            Properties = new ReadOnlyDictionary<string, object>(properties);
        }
    }
}