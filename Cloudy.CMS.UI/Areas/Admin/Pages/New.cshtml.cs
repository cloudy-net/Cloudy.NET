using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.UI.FormSupport;
using Cloudy.CMS.ContextSupport;
using System.Threading.Tasks;
using System.Linq;
using Cloudy.CMS.EntitySupport.PrimaryKey;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
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

        void BindData(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
        }

        public void OnGet(string contentType)
        {
            BindData(contentType);
        }
    }
}
