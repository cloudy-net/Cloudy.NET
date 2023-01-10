using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntityTypeSupport.Naming;
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
        IEntityTypeProvider EntityTypeProvider { get; }
        IListColumnProvider ListColumnProvider { get; }
        IListFilterProvider ListFilterProvider { get; }
        IEntityTypeNameProvider EntityTypeNameProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public ListModel(IEntityTypeProvider entityTypeProvider, IListColumnProvider listColumnProvider, IListFilterProvider listFilterProvider, IEntityTypeNameProvider entityTypeNameProvider, IContextCreator contextCreator, IPrimaryKeyGetter primaryKeyGetter)
        {
            EntityTypeProvider = entityTypeProvider;
            ListColumnProvider = listColumnProvider;
            ListFilterProvider = listFilterProvider;
            EntityTypeNameProvider = entityTypeNameProvider;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
        }

        public IEnumerable<ListColumnDescriptor> Columns { get; set; }
        public IEnumerable<ListFilterDescriptor> Filters { get; set; }
        public EntityTypeDescriptor EntityType { get; set; }
        public EntityTypeName EntityTypeName { get; set; }

        public async Task<IActionResult> OnGet(string entityType)
        {
            EntityType = EntityTypeProvider.Get(entityType);
            Columns = ListColumnProvider.Get(EntityType.Type);
            Filters = ListFilterProvider.Get(EntityType.Type);
            EntityTypeName = EntityTypeNameProvider.Get(EntityType.Type);

            if (EntityType.IsSingleton)
            {
                var context = ContextCreator.CreateFor(EntityType.Type);
                var entity = await ((IQueryable)context.GetDbSet(EntityType.Type)).Cast<object>().FirstOrDefaultAsync();

                if(entity != null)
                {
                    return Redirect(Url.Page("Edit", new { area = "Admin", EntityType = EntityType.Name, keys = PrimaryKeyGetter.Get(entity) }));
                }

                return Redirect(Url.Page("New", new { area = "Admin", EntityType = EntityType.Name }));
            }

            return Page();
        }
    }
}
