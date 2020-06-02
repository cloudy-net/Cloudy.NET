using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentInterface
    {
        public string Id { get; set; }
        public IDictionary<string, object> Properties { get; set; }

        public static DocumentInterface CreateFrom(string id, IDictionary<string, object> properties)
        {
            return new DocumentInterface
            {
                Id = id,
                Properties = new ReadOnlyDictionary<string, object>(properties),
            };
        }
    }
}