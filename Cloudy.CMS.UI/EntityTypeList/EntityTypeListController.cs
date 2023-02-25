using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntityTypeSupport.Naming;
using Cloudy.CMS.SingletonSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.EntityTypeList
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class EntityTypeListController : Controller
    {
        private readonly IEntityTypeNameProvider entityTypeNameProvider;
        private readonly IEntityTypeProvider entityTypeProvider;
        private readonly ISingletonGetter singletonGetter;

        public EntityTypeListController(
            IEntityTypeProvider entityTypeProvider,
            IEntityTypeNameProvider entityTypeNameProvider,
            ISingletonGetter singletonGetter)
        {
            this.entityTypeProvider = entityTypeProvider;
            this.entityTypeNameProvider = entityTypeNameProvider;
            this.singletonGetter = singletonGetter;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/entity-type-list/result")]
        public async Task<IEnumerable<EntityTypeItem>> ListResult()
        {
            var entityTypes = entityTypeProvider.GetAll().Where(t => t.IsIndependent);
            var entityTypeItems = entityTypes.Select(async entityType => new EntityTypeItem(
                entityTypeNameProvider.Get(entityType.Type).PluralName,
                entityType.Type.GetCustomAttribute<DisplayAttribute>()?.Description,
                entityType.IsSingleton,
                await GetLink(entityType).ToListAsync()
            ));

            return await Task.WhenAll(entityTypeItems);
        }

        private async IAsyncEnumerable<Link> GetLink(EntityTypeDescriptor entityType)
        {
            if (entityType.IsSingleton)
            {
                var action = await singletonGetter.Get(entityType.Type) is null ? "New" : "Edit";

                yield return new Link
                {
                    Action = "List",
                    EntityTypeName = entityType.Name,
                    Text = action,
                };
            }
            else
            {
                yield return new Link
                {
                    Action = "List",
                    EntityTypeName = entityType.Name,
                    Text = "List all",
                };

                yield return new Link
                {
                    Action = "New",
                    EntityTypeName = entityType.Name,
                    Text = "New",
                };
            }
        }

        public class Link
        {
            public string Text { get; set; }
            public string Action { get; set; }
            public string EntityTypeName { get; set; }
        };

        public record EntityTypeItem(
            string PluralName,
            string Description,
            bool IsSingleton,
            IEnumerable<Link> Links
        );
    }
}
