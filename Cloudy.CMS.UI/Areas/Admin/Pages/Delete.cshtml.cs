using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using System.Linq;
using Cloudy.CMS.EntitySupport.PrimaryKey;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class DeleteModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }

        public DeleteModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider, IContextCreator contextCreator, IPrimaryKeyConverter primaryKeyConverter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            ContextCreator = contextCreator;
            PrimaryKeyConverter = primaryKeyConverter;
        }

        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }
        public object Instance { get; set; }

        async Task BindData(string contentType, string[] keys)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
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

        public async Task<IActionResult> OnPost(string contentType, string[] keys, [FromForm] IFormCollection form)
        {
            await BindData(contentType, keys).ConfigureAwait(false);

            if (Instance == null)
            {
                return NotFound($"Could not find instance of type {contentType} and key{(keys.Length > 1 ? "s" : null)} {string.Join(", ", keys)}");
            }

            var context = ContextCreator.CreateFor(ContentType.Type);
            context.Context.Remove(Instance);
            await context.Context.SaveChangesAsync().ConfigureAwait(false);

            return Redirect(Url.Page("List", new { area = "Admin", ContentType = ContentType.Name }));
        }
    }
}
