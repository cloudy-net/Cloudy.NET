using Cloudy.CMS.AssemblySupport;
using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach(var assembly in AssemblyProvider.Assemblies)
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

            var duplicates = result.GroupBy(c => c.Id).Where(c => c.Count() > 1).ToList();

            if (duplicates.Any())
            {
                throw new Exception($"Admin controls contains duplicate ids: {string.Join(", ", duplicates.Select(d => $"\"{d.Key}\" is used by both {string.Join(" and ", d.Select(d2 => d2.Type))}"))}");
            }

            return result.AsReadOnly();
        }
    }
}
