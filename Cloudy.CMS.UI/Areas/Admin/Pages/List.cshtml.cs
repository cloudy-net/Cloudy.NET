using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.UI.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class ListModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IListColumnProvider ListColumnProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }

        public ListModel(IContentTypeProvider contentTypeProvider, IListColumnProvider listColumnProvider, IContentTypeNameProvider contentTypeNameProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            ListColumnProvider = listColumnProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
        }

        public IEnumerable<ListColumnDescriptor> Columns { get; set; }
        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }

        public void OnGet(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            Columns = ListColumnProvider.Get(ContentType.Type);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
        }
    }
}
