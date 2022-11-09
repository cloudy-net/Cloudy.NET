using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormViewComponent : ViewComponent
    {
        IFieldProvider FieldProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }

        public FormViewComponent(IFieldProvider fieldProvider, IPropertyDefinitionProvider propertyDefinitionProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IContentTypeProvider contentTypeProvider)
        {
            FieldProvider = fieldProvider;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            ContentTypeProvider = contentTypeProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string contentType, object instance)
        {
            var type = ContentTypeProvider.Get(contentType);

            return View("Form", new FormViewModel(
                FieldProvider.Get(contentType),
                PropertyDefinitionProvider.GetFor(type.Name),
                PrimaryKeyPropertyGetter.GetFor(type.Type).Select(p => p.Name).ToList().AsReadOnly(),
                type,
                instance
            ));
        }
    }
}
