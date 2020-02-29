using System;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public class TypeControlMapping
    {
        public Type Type { get; }
        public string ControlId { get; }

        public TypeControlMapping(Type type, string controlId)
        {
            Type = type;
            ControlId = controlId;
        }
    }
}