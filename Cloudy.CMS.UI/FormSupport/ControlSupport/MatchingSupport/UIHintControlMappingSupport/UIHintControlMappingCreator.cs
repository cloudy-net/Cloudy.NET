using Poetry.ComponentSupport;
using Poetry.UI.FormSupport.UIHintSupport;
using Poetry.UI.FormSupport.UIHintSupport.ParserSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport
{
    public class UIHintControlMappingCreator : IUIHintControlMappingCreator
    {
        IComponentProvider ComponentProvider { get; }
        IUIHintDefinitionParser UIHintDefinitionParser { get; }

        public UIHintControlMappingCreator(IComponentProvider componentProvider, IUIHintDefinitionParser uiHintDefinitionParser)
        {
            ComponentProvider = componentProvider;
            UIHintDefinitionParser = uiHintDefinitionParser;
        }

        public IEnumerable<UIHintControlMapping> Create()
        {
            var result = new List<UIHintControlMapping>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
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
