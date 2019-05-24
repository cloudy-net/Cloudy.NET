using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class DocumentFacet
    {
        public string Language { get; set; }
        public IDictionary<string, DocumentInterface> Interfaces { get; set; }
        public IDictionary<string, object> Properties { get; set; }

        public DocumentFacet(string language, IEnumerable<DocumentInterface> interfaces, IDictionary<string, object> properties)
        {
            Language = language;
            Interfaces = new ReadOnlyDictionary<string, DocumentInterface>(interfaces.ToDictionary(i => i.Id, i => i));
            Properties = new ReadOnlyDictionary<string, object>(properties);
        }
    }
}
