using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class PolymorphicFormDeserializationCandidateCreator : IPolymorphicCandidateCreator
    {
        IFormProvider FormProvider { get; }

        public PolymorphicFormDeserializationCandidateCreator(IFormProvider formProvider)
        {
            FormProvider = formProvider;
        }

        public IEnumerable<PolymorphicCandidateDescriptor> Create()
        {
            var result = new List<PolymorphicCandidateDescriptor>();

            foreach(var form in FormProvider.GetAll())
            {
                if (typeof(IContent).IsAssignableFrom(form.Type))
                {
                    continue;
                }

                result.Add(new PolymorphicCandidateDescriptor(form.Id, form.Type));
            }

            return result.AsReadOnly();
        }
    }
}
