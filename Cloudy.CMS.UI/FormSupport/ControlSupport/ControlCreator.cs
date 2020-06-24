using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public class ControlCreator : IControlCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public ControlCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<ControlDescriptor> Create()
        {
            var result = new List<ControlDescriptor>();

            foreach(var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
                {
                    var attribute = type.GetCustomAttribute<ControlAttribute>();

                    if(attribute == null)
                    {
                        continue;
                    }

                    result.Add(new ControlDescriptor(attribute.Id, attribute.ModulePath, type));
                }
            }

            return result.AsReadOnly();
        }
    }
}
