using Cloudy.CMS.PropertyAccess;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.RuntimeSupport
{
    public class FormInstanceInitializer : IFormInstanceInitializer
    {
        IFormProvider FormProvider { get; }
        IFieldProvider FieldProvider { get; }
        IPropertySetter PropertySetter { get; }

        public FormInstanceInitializer(IFormProvider formProvider, IFieldProvider fieldProvider, IPropertySetter propertySetter)
        {
            FormProvider = formProvider;
            FieldProvider = fieldProvider;
            PropertySetter = propertySetter;
        }

        public void Initialize(object instance, FormDescriptor form)
        {
            foreach (var field in FieldProvider.GetAllFor(form.Id))
            {
                if (field.IsSortable)
                {
                    PropertySetter.SetProperty(form.Type, instance, field.Id, Activator.CreateInstance(typeof(List<>).MakeGenericType(field.Type)));

                    continue;
                }

                var nestedForm = FormProvider.Get(field.Type);

                if (nestedForm == null)
                {
                    continue;
                }

                var value = Activator.CreateInstance(field.Type);

                Initialize(value, nestedForm);

                PropertySetter.SetProperty(form.Type, instance, field.Id, value);
            }
        }
    }
}
