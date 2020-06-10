using Cloudy.CMS.UI.ScriptSupport;
using Cloudy.CMS.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public class ControlStyleCreator : IStyleCreator
    {
        IControlProvider ControlProvider { get; }

        public ControlStyleCreator(IControlProvider controlProvider)
        {
            ControlProvider = controlProvider;
        }

        public IEnumerable<StyleDescriptor> Create()
        {
            var result = new List<StyleDescriptor>();

            foreach(var control in ControlProvider.GetAll())
            {
                foreach(var style in control.Type.GetCustomAttributes<StyleAttribute>())
                {
                    result.Add(new StyleDescriptor(style.Path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
