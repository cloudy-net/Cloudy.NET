using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Mime;
using System.Linq;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class EditModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        IContextProvider ContextProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IInstanceUpdater InstanceUpdater { get; }

        public EditModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IContextProvider contextProvider, IPrimaryKeyConverter primaryKeyConverter, IInstanceUpdater instanceUpdater)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            ContextProvider = contextProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            InstanceUpdater = instanceUpdater;
        }

        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }
        public object Instance { get; set; }

        async Task BindData(string contentType, string[] keys)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
            var keyValues = PrimaryKeyConverter.Convert(keys, ContentType.Type);
            var context = ContextProvider.GetFor(ContentType.Type);
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

            var primaryKeyNames = PrimaryKeyPropertyGetter.GetFor(ContentType.Type).Select(p => p.Name).ToList();

            var context = ContextProvider.GetFor(ContentType.Type);
            InstanceUpdater.Update(ContentType, primaryKeyNames, Instance, form);
            await context.Context.SaveChangesAsync().ConfigureAwait(false);

            return Redirect(Url.Page("List", new { area = "Admin", ContentType = ContentType.Name }));
        }
    }
}
