using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntityTypeSupport.Naming;
using Cloudy.CMS.Naming;
using Cloudy.CMS.UI.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    [Authorize("adminarea")]
    [Area("Admin")]
    [ResponseCache(NoStore = true)]
    public class DeleteEntityController : Controller
    {
        IEntityTypeNameProvider EntityTypeNameProvider { get; }
        IEntityTypeProvider EntityTypeProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContextCreator ContextCreator { get; }
        INameGetter NameGetter { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }

        public DeleteEntityController(
            IEntityTypeProvider entityTypeProvider,
            IPrimaryKeyConverter primaryKeyConverter,
            IContextCreator contextCreator,
            IPrimaryKeyPropertyGetter primaryKeyPropertyGetter,
            IEntityTypeNameProvider entityTypeNameProvider,
            INameGetter nameGetter)
        {
            EntityTypeProvider = entityTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContextCreator = contextCreator;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            EntityTypeNameProvider = entityTypeNameProvider;
            NameGetter = nameGetter;
        }

        [HttpGet]
        [Route("/{area}/api/form/entity/delete")]
        public async Task<DeleteEntityViewModel> Get([FromQuery] string entityTypeName, [FromQuery] string[] keys)
        {
            var entityType = EntityTypeProvider.Get(entityTypeName);
            var keyValues = PrimaryKeyConverter.Convert(keys, entityType.Type);
            var context = ContextCreator.CreateFor(entityType.Type);
            var instance = await context.Context.FindAsync(entityType.Type, keyValues).ConfigureAwait(false);
            var primaryKeys = PrimaryKeyPropertyGetter.GetFor(entityType.Type);
            var primaryKeysWithValues = primaryKeys
                .Select(pk => new { pk.Name, Value = pk.GetValue(instance)?.ToString() })
                .ToDictionary(item => item.Name, item => item.Value);

            return new DeleteEntityViewModel
            {
                InstanceName = NameGetter.GetName(instance),
                PluralLowerCaseName = EntityTypeNameProvider.Get(entityType.Type).PluralLowerCaseName,
                PrimaryKeysWithValues = primaryKeysWithValues
            };
        }

        [HttpDelete]
        [Route("/{area}/api/form/entity/delete")]
        public async Task<DeleteResponse> Delete([FromQuery] string entityTypeName, [FromQuery] string[] keys)
        {
            var entityType = EntityTypeProvider.Get(entityTypeName);
            if (entityType.IsSingleton) return new DeleteResponse
            {
                RedirectUrl = UrlBuilder.Build(keys: null, "List", entityTypeName)
            };

            var keyValues = PrimaryKeyConverter.Convert(keys, entityType.Type);
            var context = ContextCreator.CreateFor(entityType.Type);
            var instance = await context.Context.FindAsync(entityType.Type, keyValues).ConfigureAwait(false);

            if (instance is null) return new DeleteResponse
            {
                ErrorMessage = $"Could not find instance of type {entityTypeName} and key{(keys.Length > 1 ? "s" : null)} {string.Join(", ", keys)}"
            };

            context.Context.Remove(instance);
            await context.Context.SaveChangesAsync().ConfigureAwait(false);

            return new DeleteResponse
            {
                RedirectUrl = UrlBuilder.Build(keys: null, "List", entityTypeName)
            };
        }

        public class DeleteEntityViewModel
        {
            public string InstanceName { get; set; }
            public string PluralLowerCaseName { get; set; }
            public Dictionary<string, string> PrimaryKeysWithValues { get; set; }
        }

        public class DeleteResponse
        {
            public string ErrorMessage { get; set; }
            public string RedirectUrl { get; set; }
        }
    }
}
