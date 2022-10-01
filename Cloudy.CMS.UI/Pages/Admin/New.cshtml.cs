using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Cloudy.CMS.UI.Pages.Admin
{
    [Authorize("adminarea")]
    public class NewModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }

        public NewModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
        }

        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }

        public void OnGet(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
        }
    }
}
