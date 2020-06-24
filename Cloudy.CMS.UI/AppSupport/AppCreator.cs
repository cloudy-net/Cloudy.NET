using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppCreator : IAppCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public AppCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<AppDescriptor> Create()
        {
            var result = new List<AppDescriptor>();

            foreach (var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
                {
                    var attribute = type.GetCustomAttribute<AppAttribute>();

                    if (attribute == null)
                    {
                        continue;
                    }

                    var displayAttribute = type.GetCustomAttribute<DisplayAttribute>();

                    var name = displayAttribute?.GetName() ?? attribute.Id;

                    result.Add(new AppDescriptor(
                        attribute.Id,
                        attribute.ModulePath,
                        name
                    ));
                }
            }

            return result.AsReadOnly();
        }
    }
}
