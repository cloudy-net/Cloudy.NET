using Cloudy.CMS.UI.FormSupport.ControlSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public class TypeControlMatch : IControlMatch
    {
        public string Id { get; }
        public string Type { get; }

        public TypeControlMatch(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
