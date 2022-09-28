using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.Pages.Admin
{
    public class ListModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IListColumnProvider ListColumnProvider { get; }

        public ListModel(IContentTypeProvider contentTypeProvider, IListColumnProvider listColumnProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            ListColumnProvider = listColumnProvider;
        }

        public IEnumerable<ListColumnDescriptor> Columns { get; set; }
        public ContentTypeDescriptor ContentType { get; set; }

        public void OnGet(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            Columns = ListColumnProvider.Get(ContentType.Type);
        }

    }
}
