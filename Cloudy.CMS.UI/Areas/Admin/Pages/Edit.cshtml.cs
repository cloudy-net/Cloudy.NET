using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Cloudy.CMS.ContentSupport.RepositorySupport.Methods;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentSupport.RepositorySupport.Context;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class EditModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IContextProvider ContextProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }

        public EditModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider, IContextProvider contextProvider, IPrimaryKeyConverter primaryKeyConverter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            ContextProvider = contextProvider;
            PrimaryKeyConverter = primaryKeyConverter;
        }

        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }
        public object Instance { get; set; }

        public async Task<IActionResult> OnGet(string contentType, string[] keys)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
            var keyValues = PrimaryKeyConverter.Convert(keys, ContentType.Type);
            var context = ContextProvider.GetFor(ContentType.Type);
            Instance = await context.Context.FindAsync(ContentType.Type, keyValues).ConfigureAwait(false);

            if(Instance == null)
            {
                return NotFound($"Could not find instance of type {contentType} and key{(keys.Length > 1 ? "s" : null)} {string.Join(", ", keys)}");
            }

            return Page();
        }
    }
}
