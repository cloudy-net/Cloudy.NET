using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class EditModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }

        public EditModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider, IContextCreator contextCreator, IPrimaryKeyConverter primaryKeyConverter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            ContextCreator = contextCreator;
            PrimaryKeyConverter = primaryKeyConverter;
        }

        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }
        public IEnumerable<string> Keys { get; set; }
        public object Instance { get; set; }

        async Task BindData(string contentType, string[] keys)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
            Keys = keys;
            var keyValues = PrimaryKeyConverter.Convert(keys, ContentType.Type);
            var context = ContextCreator.CreateFor(ContentType.Type);
            Instance = await context.Context.FindAsync(ContentType.Type, keyValues).ConfigureAwait(false);
        }

        public async Task<IActionResult> OnGet(string contentType, string[] keys)
        {
            await BindData(contentType, keys).ConfigureAwait(false);

            if (Instance == null)
            {
                return NotFound($"Could not find instance of type {contentType} and key{(keys.Length > 1 ? "s" : null)} {string.Join(", ", keys)}");
            }

            return Page();
        }
    }
}
