using Cloudy.CMS.ComponentSupport;
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
        IComponentProvider ComponentProvider { get; }

        public AppCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<AppDescriptor> Create()
        {
            var result = new List<AppDescriptor>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
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
                        component.Id,
                        attribute.ModulePath,
                        name
                    ));
                }
            }

            return result.AsReadOnly();
        }
    }
}
