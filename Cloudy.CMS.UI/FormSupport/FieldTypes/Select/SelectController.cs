using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.EntitySupport.Reference;
using Cloudy.CMS.Naming;
using Cloudy.CMS.UI.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport.FieldTypes
{
    public class SelectController : Controller
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        INameGetter NameGetter { get; }

        public SelectController(IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeProvider contentTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine, IPrimaryKeyGetter primaryKeyGetter, INameGetter nameGetter)
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
        public async Task<SelectResult> List(string contentType, string filter, int page, int pageSize)
        {
            var type = ContentTypeProvider.Get(contentType);

            using var context = ContextCreator.CreateFor(type.Type);

            var dbSet = (IQueryable)context.GetDbSet(type.Type).DbSet;

            if (filter != null)
            {
                if (type.Type.IsAssignableTo(typeof(INameable)))
                {
                    dbSet = dbSet.Where($"Name.Contains(@0)", filter);
                }
            }

            var totalCount = await dbSet.CountAsync().ConfigureAwait(false);

            dbSet = dbSet.Page(page, pageSize);

            var result = new List<SelectResultItem>();

            var propertyDefinitions = PropertyDefinitionProvider.GetFor(type.Name);

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                result.Add(new SelectResultItem(
                    NameGetter.GetName(instance),
                    JsonSerializer.Serialize(PrimaryKeyGetter.Get(instance))
                ));
            }

            return new SelectResult(
                result,
                totalCount
            );
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/controls/select/getcard")]
        public async Task<SelectResultItem> GetCard(string contentType, string reference, [FromServices] IReferenceDeserializer referenceDeserializer)
        {
            var type = ContentTypeProvider.Get(contentType);

            using var context = ContextCreator.CreateFor(type.Type);

            var instance = await context.Context.FindAsync(type.Type, referenceDeserializer.Deserialize(type.Type, reference)).ConfigureAwait(false);
            
            return new SelectResultItem(
                NameGetter.GetName(instance),
                JsonSerializer.Serialize(PrimaryKeyGetter.Get(instance))
            );
        }

        public record SelectResultItem(
            string Name,
            string Reference
        );

        public record SelectResult(
            IEnumerable<SelectResultItem> Items,
            int TotalCount
        );
    }
}
