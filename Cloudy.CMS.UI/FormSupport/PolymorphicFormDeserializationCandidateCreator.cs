using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class PolymorphicFormDeserializationCandidateCreator : IPolymorphicCandidateCreator
    {
        IFormProvider FormProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }

        public PolymorphicFormDeserializationCandidateCreator(IFormProvider formProvider, IContentTypeProvider contentTypeProvider)
        {
            FormProvider = formProvider;
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<PolymorphicCandidateDescriptor> Create()
        {
            var result = new List<PolymorphicCandidateDescriptor>();

            foreach(var form in FormProvider.GetAll())
            {
                if (ContentTypeProvider.Get(form.Type) != null)
                {
                    continue;
                }

                result.Add(new PolymorphicCandidateDescriptor(form.Id, form.Type));
            }

            return result.AsReadOnly();
        }
    }
}
