using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntityTypeSupport.Naming;
using Cloudy.CMS.UI.List.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class ListSettingsController : Controller
    {
        IEntityTypeProvider EntityTypeProvider { get; }
        IListColumnProvider ListColumnProvider { get; }
        IListFilterProvider ListFilterProvider { get; }
        IEntityTypeNameProvider EntityTypeNameProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public ListSettingsController(IEntityTypeProvider entityTypeProvider, IListColumnProvider listColumnProvider, IListFilterProvider listFilterProvider, IEntityTypeNameProvider entityTypeNameProvider, IContextCreator contextCreator, IPrimaryKeyGetter primaryKeyGetter)
        {
            EntityTypeProvider = entityTypeProvider;
            ListColumnProvider = listColumnProvider;
            ListFilterProvider = listFilterProvider;
            EntityTypeNameProvider = entityTypeNameProvider;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/list/settings")]
        public async Task<ListSettings> Settings([FromQuery] string entityTypeName)
        {
            var entityType = EntityTypeProvider.Get(entityTypeName);

            var listSettings = new ListSettings
            {
                Columns = ListColumnProvider.Get(entityType.Type),
                Filters = ListFilterProvider.Get(entityType.Type),
                EntityTypeName = entityTypeName,
                EntityTypePluralName = EntityTypeNameProvider.Get(entityType.Type).PluralName,
                EditLink = $"/Admin/Edit/{entityTypeName}",
                DeleteLink = $"/Admin/Delete/{entityTypeName}"
            };

            if (entityType.IsSingleton)
            {
                //var context = ContextCreator.CreateFor(entityType.Type);
                //var entity = await ((IQueryable)context.GetDbSet(entityType.Type)).Cast<object>().FirstOrDefaultAsync();

                // Fix - Does not work without Razor pages

                //listSettings.RedirectUrl = entity is null
                //    ? LinkGenerator.GetPathByPage("/New", null, new { Area = "Admin", EntityType = entityTypeName })
                //    : LinkGenerator.GetPathByPage("/Edit", null, new { Area = "Admin", EntityType = entityTypeName, keys = PrimaryKeyGetter.Get(entity) });
            }

            return listSettings;
        }

        public class ListSettings
        {
            public IEnumerable<ListColumnDescriptor> Columns { get; set; }
            public IEnumerable<ListFilterDescriptor> Filters { get; set; }
            public string EntityTypeName { get; set; }
            public string EntityTypePluralName { get; set; }
            public int PageSize => 15;
            public string RedirectUrl { get; set; }
            public string EditLink { get; set; }
            public string DeleteLink { get; set; }
        }
    }
}
