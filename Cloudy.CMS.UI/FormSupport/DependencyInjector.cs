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
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IFieldCreator, FieldCreator>();
            services.AddScoped<IFieldProvider, FieldProvider>();
            services.AddSingleton<IFormCreator, FormCreator>();
            services.AddScoped<IFormProvider, FormProvider>();
            services.AddSingleton<IControlProvider, ControlProvider>();
            services.AddSingleton<IControlCreator, ControlCreator>();
            services.AddScoped<IControlMatcher, ControlMatcher>();
            services.AddSingleton<IUIHintParser, UIHintParser>();
            services.AddSingleton<IUIHintParameterValueParser, UIHintParameterValueParser>();
            services.AddSingleton<IUIHintDefinitionParser, UIHintDefinitionParser>();
            services.AddScoped<IUIHintControlMatcher, UIHintControlMatcher>();
            services.AddSingleton<IUIHintControlMatchEvaluator, UIHintControlMatchEvaluator>();
            services.AddSingleton<IExpressionParser, ExpressionParser>();
            services.AddSingleton<IUIHintControlMatchCreator, UIHintControlMatchCreator>();
            services.AddScoped<IUIHintControlMappingProvider, UIHintControlMappingProvider>();
            services.AddScoped<ITypeControlMatcher, TypeControlMatcher>();
            services.AddScoped<IPolymorphicFormFinder, PolymorphicFormFinder>();
            services.AddSingleton<IInterfacePropertyMapper, InterfacePropertyMapper>();
            services.AddSingleton<IPropertyAttributeInheritor, PropertyAttributeInheritor>();
        }
    }
}
