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
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using System.Linq;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class NewModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        IInstanceUpdater InstanceUpdater { get; }
        IContextProvider ContextProvider { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public NewModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IInstanceUpdater instanceUpdater, IContextProvider contextProvider, IPrimaryKeyGetter primaryKeyGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            InstanceUpdater = instanceUpdater;
            ContextProvider = contextProvider;
            PrimaryKeyGetter = primaryKeyGetter;
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

        public async Task<IActionResult> OnPost(string contentType, [FromForm] IFormCollection form)
        {
            BindData(contentType);

            var primaryKeyNames = PrimaryKeyPropertyGetter.GetFor(ContentType.Type).Select(p => p.Name).ToList();

            var context = ContextProvider.GetFor(ContentType.Type);
            var instance = Activator.CreateInstance(ContentType.Type);
            InstanceUpdater.Update(ContentType, primaryKeyNames, instance, form);
            await context.Context.AddAsync(instance).ConfigureAwait(false);
            await context.Context.SaveChangesAsync().ConfigureAwait(false);

            return Redirect(Url.Page("Edit", new { area = "Admin", ContentType = ContentType.Name, keys = PrimaryKeyGetter.Get(instance) }));
        }
    }
}
