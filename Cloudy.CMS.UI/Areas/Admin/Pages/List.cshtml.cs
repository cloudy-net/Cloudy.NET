using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.Name;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.UI.List;
using Cloudy.CMS.UI.List.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.Areas.Admin.Pages
{
    [Authorize("adminarea")]
    public class ListModel : PageModel
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IListColumnProvider ListColumnProvider { get; }
        IListFilterProvider ListFilterProvider { get; }
        IContentTypeNameProvider ContentTypeNameProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public ListModel(IContentTypeProvider contentTypeProvider, IListColumnProvider listColumnProvider, IListFilterProvider listFilterProvider, IContentTypeNameProvider contentTypeNameProvider, IContextCreator contextCreator, IPrimaryKeyGetter primaryKeyGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            ListColumnProvider = listColumnProvider;
            ListFilterProvider = listFilterProvider;
            ContentTypeNameProvider = contentTypeNameProvider;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
        }

        public IEnumerable<ListColumnDescriptor> Columns { get; set; }
        public IEnumerable<ListFilterDescriptor> Filters { get; set; }
        public ContentTypeDescriptor ContentType { get; set; }
        public ContentTypeName ContentTypeName { get; set; }

        public async Task<IActionResult> OnGet(string contentType)
        {
            ContentType = ContentTypeProvider.Get(contentType);
            Columns = ListColumnProvider.Get(ContentType.Type);
            Filters = ListFilterProvider.Get(ContentType.Type);
            ContentTypeName = ContentTypeNameProvider.Get(ContentType.Type);

            if (ContentType.IsSingleton)
            {
                var context = ContextCreator.CreateFor(ContentType.Type);
                var entity = await ((IQueryable)context.GetDbSet(ContentType.Type)).Cast<object>().FirstOrDefaultAsync();

                if(entity != null)
                {
                    return Redirect(Url.Page("Edit", new { area = "Admin", ContentType = ContentType.Name, keys = PrimaryKeyGetter.Get(entity) }));
                }

                return Redirect(Url.Page("New", new { area = "Admin", ContentType = ContentType.Name }));
            }

            return Page();
        }
    }
}
