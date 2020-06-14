using Cloudy.CMS.ComponentSupport;
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
        IContentFormIdGenerator ContentFormIdGenerator { get; }

        public ContentFormCreator(IContentTypeProvider contentTypeRepository, IContentFormIdGenerator contentFormIdGenerator)
        {
            ContentTypeRepository = contentTypeRepository;
            ContentFormIdGenerator = contentFormIdGenerator;
        }

        public IEnumerable<FormDescriptor> CreateAll()
        {
            var result = new List<FormDescriptor>();

            foreach (var type in ContentTypeRepository.GetAll())
            {
                result.Add(new FormDescriptor(ContentFormIdGenerator.Generate(type), type.Type));
            }

            return result.AsReadOnly();
        }
    }
}
