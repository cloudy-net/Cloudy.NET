using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.UI.FormSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public class ContentFormCreator : IFormCreator
    {
        IContentTypeProvider ContentTypeRepository { get; }

        public ContentFormCreator(IContentTypeProvider contentTypeRepository)
        {
            ContentTypeRepository = contentTypeRepository;
        }

        public IEnumerable<FormDescriptor> CreateAll()
        {
            var result = new List<FormDescriptor>();

            foreach (var contentType in ContentTypeRepository.GetAll())
            {
                result.Add(new FormDescriptor(contentType.Id, contentType.Type));
            }

            return result.AsReadOnly();
        }
    }
}
