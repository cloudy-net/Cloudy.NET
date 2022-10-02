using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using Cloudy.CMS.UI.FormSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.Methods;
using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.Pages.Admin
{
    [Authorize("adminarea")]
    public class NewModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IInstanceUpdater InstanceUpdater { get; }
        IContextProvider ContextProvider { get; }

        public NewModel(IContentTypeProvider contentTypeProvider, IContentTypeNameProvider contentTypeNameProvider, IInstanceUpdater instanceUpdater, IContextProvider contextProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            InstanceUpdater = instanceUpdater;
            ContextProvider = contextProvider;
        }

        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }

        public void OnGet(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);
        }

        public async Task OnPost(string contentType, [FromForm] IFormCollection form)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);

            var context = ContextProvider.GetFor(ContentType.Type);
            var instance = Activator.CreateInstance(ContentType.Type);
            InstanceUpdater.Update(ContentType.Name, instance, form);
            await context.Context.AddAsync(instance).ConfigureAwait(false);
            await context.Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
