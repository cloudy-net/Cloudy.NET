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
using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormSupportDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IFieldCreator, FieldCreator>();
            services.AddSingleton<IFieldProvider, FieldProvider>();
            services.AddSingleton<IFormCreator, FormCreator>();
            services.AddSingleton<IFormProvider, FormProvider>();
            services.AddSingleton<IControlProvider, ControlProvider>();
            services.AddSingleton<IControlCreator, ControlCreator>();
            services.AddSingleton<IControlMatcher, ControlMatcher>();
            services.AddSingleton<IUIHintParser, UIHintParser>();
            services.AddSingleton<IUIHintParameterValueParser, UIHintParameterValueParser>();
            services.AddSingleton<IUIHintDefinitionParser, UIHintDefinitionParser>();
            services.AddSingleton<IUIHintControlMatcher, UIHintControlMatcher>();
            services.AddSingleton<IUIHintControlMatchEvaluator, UIHintControlMatchEvaluator>();
            services.AddSingleton<IExpressionParser, ExpressionParser>();
            services.AddSingleton<IUIHintControlMatchCreator, UIHintControlMatchCreator>();
            services.AddSingleton<IUIHintControlMappingProvider, UIHintControlMappingProvider>();
            services.AddSingleton<ITypeControlMatcher, TypeControlMatcher>();
            services.AddSingleton<IPolymorphicFormFinder, PolymorphicFormFinder>();
            services.AddSingleton<IInterfacePropertyMapper, InterfacePropertyMapper>();
            services.AddSingleton<IPropertyAttributeInheritor, PropertyAttributeInheritor>();
        }
    }
}
