using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.UIHintControlMappingSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.UI.FormSupport.UIHintSupport;
using Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport;

namespace Cloudy.CMS.UI.FormSupport
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
            container.RegisterSingleton<IUIHintControlMatchEvaluator, UIHintControlMatchEvaluator>();
            container.RegisterSingleton<IExpressionParser, ExpressionParser>();
            container.RegisterSingleton<IUIHintControlMatchCreator, UIHintControlMatchCreator>();
            container.RegisterSingleton<IUIHintControlMappingProvider, UIHintControlMappingProvider>();
            container.RegisterSingleton<ITypeControlMatcher, TypeControlMatcher>();
            container.RegisterSingleton<IInterfacePropertyMapper, InterfacePropertyMapper>();
            container.RegisterSingleton<IPropertyAttributeInheritor, PropertyAttributeInheritor>();
        }
    }
}
