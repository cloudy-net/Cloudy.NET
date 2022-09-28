using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.Pages.Admin
{
    public class ListModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public ListModel(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<ListPageColumn> Columns { get; set; }
        public ContentTypeDescriptor ContentType { get; set; }

        public void OnGet(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
        }

        public record ListPageColumn(string Name);
    }
}
