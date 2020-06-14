using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.ContentAppSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport
{
    public class PolymorphicFormFinder : IPolymorphicFormFinder
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IFormProvider FormProvider { get; }

        public PolymorphicFormFinder(IContentTypeProvider contentTypeProvider, IFormProvider formProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            FormProvider = formProvider;
        }

        public IEnumerable<string> FindFor(Type type)
        {
            var result = new List<string>();

            foreach (var contentType in ContentTypeProvider.GetAll())
            {
                if (type.IsAssignableFrom(contentType.Type))
                {
                    throw new CannotInlineContentTypesException(type, contentType);
                }
            }

            foreach (var form in FormProvider.GetAll())
            {
                if (type.IsAssignableFrom(form.Type))
                {
                    result.Add(form.Id);
                }
            }

            return result.AsReadOnly();
        }
    }
}
