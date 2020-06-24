using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.UI.ScriptSupport
{
    public class ScriptCreator : IScriptCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public ScriptCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<ScriptDescriptor> Create()
        {
            var result = new List<ScriptDescriptor>();

            foreach (var type in AssemblyProvider.GetAll().SelectMany(a => a.Types))
            {
                foreach (var attribute in type.GetCustomAttributes<ScriptAttribute>())
                {
                    result.Add(new ScriptDescriptor(attribute.Path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
