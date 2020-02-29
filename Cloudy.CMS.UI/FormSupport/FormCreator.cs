using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormCreator : IFormCreator
    {
        IComponentProvider ComponentProvider { get; }

        public FormCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<FormDescriptor> CreateAll()
        {
            var result = new List<FormDescriptor>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
                {
                    var formAttribute = type.GetCustomAttribute<FormAttribute>();

                    if (formAttribute == null)
                    {
                        continue;
                    }

                    result.Add(new FormDescriptor(formAttribute.Id, type));
                }
            }

            return result.AsReadOnly();
        }
    }
}
