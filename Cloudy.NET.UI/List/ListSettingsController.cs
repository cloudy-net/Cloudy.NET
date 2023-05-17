using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.EntityTypeSupport.Naming;
using Cloudy.NET.UI.Layout;
using Cloudy.NET.UI.List.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
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
        public async Task<IDictionary<string, ListSettings>> Settings()
        {
            var result = new Dictionary<string, ListSettings>();

            foreach (var entityType in EntityTypeProvider.GetAll())
            {
                var listSettings = new ListSettings
                {
                    Columns = ListColumnProvider.Get(entityType.Type) ?? Enumerable.Empty<ListColumnDescriptor>(),
                    Filters = ListFilterProvider.Get(entityType.Type) ?? Enumerable.Empty<ListFilterDescriptor>(),
                    EntityTypeName = entityType.Name,
                    EntityTypePluralName = EntityTypeNameProvider.Get(entityType.Type).PluralName,
                    EditLink = UrlBuilder.Build(keys: null, "Edit", entityType.Name),
                    DeleteLink = UrlBuilder.Build(keys: null, "Delete", entityType.Name)
                };

                if (entityType.IsSingleton)
                {
                    var context = ContextCreator.CreateFor(entityType.Type);
                    var entity = await ((IQueryable)context.GetDbSet(entityType.Type)).Cast<object>().FirstOrDefaultAsync();

                    listSettings.RedirectUrl = entity is null
                        ? UrlBuilder.Build(keys: null, "New", entityType.Name)
                        : UrlBuilder.Build(keys: PrimaryKeyGetter.Get(entity), "Edit", entityType.Name);
                }

                result[entityType.Name] = listSettings;
            }

            return result;
        }
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
