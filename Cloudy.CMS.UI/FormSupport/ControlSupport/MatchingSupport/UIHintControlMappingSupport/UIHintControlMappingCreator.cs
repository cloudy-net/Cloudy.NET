using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMappingCreator : IUIHintControlMappingCreator
    {
        IAssemblyProvider AssemblyProvider { get; }
        IUIHintDefinitionParser UIHintDefinitionParser { get; }

        public UIHintControlMappingCreator(IAssemblyProvider assemblyProvider, IUIHintDefinitionParser uiHintDefinitionParser)
        {
            AssemblyProvider = assemblyProvider;
            UIHintDefinitionParser = uiHintDefinitionParser;
        }

        public IEnumerable<UIHintControlMapping> Create()
        {
            var result = new List<UIHintControlMapping>();

            foreach (var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
                {
                    var controlAttribute = type.GetCustomAttribute<ControlAttribute>();

                    if (controlAttribute == null)
                    {
                        continue;
                    }

                    foreach (var mapControlToUIHintAttribute in type.GetCustomAttributes<MapControlToUIHintAttribute>())
                    {
                        result.Add(new UIHintControlMapping(UIHintDefinitionParser.Parse(mapControlToUIHintAttribute.UIHintDefinition), controlAttribute.Id));
                    }
                }
            }

            return result.AsReadOnly();
        }
    }
}
