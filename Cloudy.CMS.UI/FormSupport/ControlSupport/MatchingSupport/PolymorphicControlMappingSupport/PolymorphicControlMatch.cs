using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport
{
    public class PolymorphicControlMatch : IControlMatch
    {
        public string Id { get; }
        public IEnumerable<string> Types { get; }
        
        public PolymorphicControlMatch(string id, IEnumerable<string> types)
        {
            Id = id;
            Types = types.ToList().AsReadOnly();
        }
    }
}
