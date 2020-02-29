using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MapControlToUIHintAttribute : Attribute
    {
        public string UIHintDefinition { get; }

        public MapControlToUIHintAttribute(string uiHintDefinition)
        {
            UIHintDefinition = uiHintDefinition;
        }
    }
}
