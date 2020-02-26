using Poetry.DependencyInjectionSupport;
using Poetry.UI.FormSupport.FieldSupport;
using Poetry.UI.FormSupport.ControlSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Poetry.UI.FormSupport.UIHintSupport;
using Poetry.UI.FormSupport.UIHintSupport.ParserSupport;

namespace Poetry.UI.FormSupport
{
    public class FormSupportDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<IFieldCreator, FieldCreator>();
            container.RegisterSingleton<IFieldProvider, FieldProvider>();
            container.RegisterSingleton<IFormCreator, FormCreator>();
            container.RegisterSingleton<IFormProvider, FormProvider>();
            container.RegisterSingleton<IControlProvider, ControlProvider>();
            container.RegisterSingleton<IControlCreator, ControlCreator>();
            container.RegisterSingleton<IControlMatcher, ControlMatcher>();
            container.RegisterSingleton<IUIHintParser, UIHintParser>();
            container.RegisterSingleton<IUIHintDefinitionParser, UIHintDefinitionParser>();
            container.RegisterSingleton<IUIHintControlMatcher, UIHintControlMatcher>();
            container.RegisterSingleton<IExpressionParser, ExpressionParser>();
            container.RegisterSingleton<IUIHintControlMatchCreator, UIHintControlMatchCreator>();
            container.RegisterSingleton<ITypeControlMatcher, TypeControlMatcher>();
            container.RegisterSingleton<IInterfacePropertyMapper, InterfacePropertyMapper>();
            container.RegisterSingleton<IPropertyAttributeInheritor, PropertyAttributeInheritor>();
        }
    }
}
