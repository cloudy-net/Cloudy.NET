using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport
{
    [DebuggerDisplay("{Id}[{Parameters.Count}]")]
    public class UIHintDefinition
    {
        public string Id { get; }
        public IList<UIHintParameterDefinition> Parameters { get; }

        public UIHintDefinition(string id, IEnumerable<UIHintParameterDefinition> parameters)
        {
            Id = id;
            Parameters = parameters.ToList().AsReadOnly();
        }
    }
}