using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poetry.UI.FormSupport.ControlSupport
{
    public class ControlReference
    {
        public string Id { get; }
        public IDictionary<string, object> Parameters { get; }

        public ControlReference(string id, IDictionary<string, object> parameters)
        {
            Id = id;
            Parameters = new ReadOnlyDictionary<string, object>(parameters);
        }
    }
}