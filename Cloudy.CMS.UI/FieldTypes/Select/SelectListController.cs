using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Naming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;

namespace Cloudy.CMS.UI.FormSupport.FieldTypes
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class SelectListController : Controller
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        INameGetter NameGetter { get; }

        public SelectListController(IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeProvider contentTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine, IPrimaryKeyGetter primaryKeyGetter, INameGetter nameGetter)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeProvider = contentTypeProvider;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
            NameGetter = nameGetter;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/select/list")]
        public async Task<IActionResult> List(string contentType, string filter, int page, int pageSize, bool simpleKey)
        {
            var type = ContentTypeProvider.Get(contentType);

            var context = ContextCreator.CreateFor(type.Type);

            var dbSet = (IQueryable)context.GetDbSet(type.Type);

            if (filter != null)
            {
                if (type.Type.IsAssignableTo(typeof(INameable)))
                {
                    dbSet = dbSet.Where($"Name.Contains(@0, \"{StringComparison.InvariantCultureIgnoreCase}\")", filter);
                }
            }

            var totalCount = await dbSet.CountAsync().ConfigureAwait(false);

            dbSet = dbSet.Page(page, pageSize);

            var result = new List<SelectResultItem>();

            var propertyDefinitions = PropertyDefinitionProvider.GetFor(type.Name);

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                var keys = PrimaryKeyGetter.Get(instance);
                result.Add(new SelectResultItem(
                    NameGetter.GetName(instance),
                    JsonSerializer.SerializeToElement(simpleKey ? keys.First() : keys),
                    (instance as IImageable)?.Image
                ));
            }

            return Json(new SelectResult(
                result,
                totalCount
            ), new JsonSerializerOptions().CloudyDefault());
        }

        public record SelectResultItem(
            string Name,
            JsonElement Reference,
            string Image
        );

        public record SelectResult(
            IEnumerable<SelectResultItem> Items,
            int TotalCount
        );
    }
}
