using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.UI.StyleSupport
{
    public class StyleCreator : IStyleCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public StyleCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<StyleDescriptor> Create()
        {
            var result = new List<StyleDescriptor>();

            foreach (var type in AssemblyProvider.GetAll().SelectMany(a => a.Types))
            {
                foreach (var attribute in type.GetCustomAttributes<StyleAttribute>())
                {
                    result.Add(new StyleDescriptor(attribute.Path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
