using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntitySupport;
using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class ContentRoutesController : Controller
    {
        private readonly IContentRouteMatcher contentRouteMatcher;
        private readonly IContextCreator contextCreator;
        private readonly IEntityTypeProvider entityTypeProvider;
        private readonly IPrimaryKeyConverter primaryKeyConverter;

        public ContentRoutesController(
            IContentRouteMatcher contentRouteMatcher,
            IContextCreator contextCreator,
            IEntityTypeProvider entityTypeProvider,
            IPrimaryKeyConverter primaryKeyConverter)
        {
            this.contentRouteMatcher = contentRouteMatcher;
            this.contextCreator = contextCreator;
            this.entityTypeProvider = entityTypeProvider;
            this.primaryKeyConverter = primaryKeyConverter;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/layout/content-routes")]
        public async Task<IEnumerable<string>> GetContentRoutes([FromQuery] string entityTypeName, [FromQuery] string[] keys)
        {
            var entityType = entityTypeProvider.Get(entityTypeName);
            var keyValues = primaryKeyConverter.Convert(keys, entityType.Type);
            var context = contextCreator.CreateFor(entityType.Type);
            var instance = await context.Context.FindAsync(entityType.Type, keyValues).ConfigureAwait(false);
            var urlSegment = (instance as IRoutable)?.UrlSegment;

            if (string.IsNullOrEmpty(urlSegment)) return Enumerable.Empty<string>();

            return contentRouteMatcher
                .GetFor(entityType.Type)
                .Select(route => $"/{route.Apply(urlSegment)}");
        }
    }
}
