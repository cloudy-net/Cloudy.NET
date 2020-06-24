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
        IAssemblyProvider AssemblyProvider { get; }

        public FormCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<FormDescriptor> CreateAll()
        {
            var result = new List<FormDescriptor>();

            foreach (var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
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
