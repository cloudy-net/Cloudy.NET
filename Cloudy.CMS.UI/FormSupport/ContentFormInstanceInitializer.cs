using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RuntimeSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.RuntimeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport
{
    public class ContentFormInstanceInitializer : IContentInstanceInitializer
    {
        IFormProvider FormProvider { get; }
        IFormInstanceInitializer FormInstanceInitializer { get; }

        public ContentFormInstanceInitializer(IFormProvider formProvider, IFormInstanceInitializer formInstanceInitializer)
        {
            FormProvider = formProvider;
            FormInstanceInitializer = formInstanceInitializer;
        }

        public void Initialize(IContent content, ContentTypeDescriptor contentType)
        {
            var form = FormProvider.Get(contentType.Type);

            FormInstanceInitializer.Initialize(content, form);
        }
    }
}
