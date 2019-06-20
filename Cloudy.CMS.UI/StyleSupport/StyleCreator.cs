using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Poetry.UI.StyleSupport
{
    public class StyleCreator : IStyleCreator
    {
        public IEnumerable<StyleDescriptor> Create(ComponentDescriptor component)
        {
            var result = new List<StyleDescriptor>();

            foreach(var attribute in component.Type.GetCustomAttributes<StyleAttribute>())
            {
                if (attribute.Path.StartsWith("/"))
                {
                    throw new AbsoluteStylePathException(component.Type, attribute.Path);
                }
                
                result.Add(new StyleDescriptor(attribute.Path));
            }

            return result.AsReadOnly();
        }
    }
}
