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
        IContentTypeProvider ContentTypeProvider { get; }

        public PolymorphicFormDeserializationCandidateCreator(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<PolymorphicCandidateDescriptor> Create()
        {
            var result = new List<PolymorphicCandidateDescriptor>();

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                result.Add(new PolymorphicCandidateDescriptor(contentType.Id, contentType.Type));
            }

            return result.AsReadOnly();
        }
    }
}
