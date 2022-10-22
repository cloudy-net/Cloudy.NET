using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Cloudy.CMS.EntitySupport.Reference;
using System.Security.Cryptography.Xml;

namespace Cloudy.CMS.UI.List
{
    public class ListResultController : Controller
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        ICompositeViewEngine CompositeViewEngine { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public ListResultController(IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeProvider contentTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine, IPrimaryKeyGetter primaryKeyGetter)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeProvider = contentTypeProvider;
            ContextCreator = contextCreator;
            CompositeViewEngine = compositeViewEngine;
            PrimaryKeyGetter = primaryKeyGetter;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/list/result")]
        public async Task<ListResultResponse> ListResult(string contentType, string columns, IDictionary<string, string> filters, int page, int pageSize, string search)
        {
            var columnNames = columns.Split(",");
            var type = ContentTypeProvider.Get(contentType);
            var propertyDefinitions = PropertyDefinitionProvider.GetFor(type.Name);
            var selectedPropertyDefinitions = columnNames.Select(n => propertyDefinitions.First(p => n == p.Name));
            
            using var context = ContextCreator.CreateFor(type.Type);

            var dbSet = (IQueryable)context.GetDbSet(type.Type);

            if (!string.IsNullOrEmpty(search))
            {
                dbSet = dbSet.Where(string.Join(" OR ", selectedPropertyDefinitions.Where(p => p.Type == typeof(string)).Select(p => $"{p.Name}.Contains(@0, \"{StringComparison.InvariantCultureIgnoreCase}\")")), search);
            }


            if (filters.Any())
            {
                var i = 0;

                var queries = new List<string>();
                var values = new List<object>();

                foreach(var filter in filters)
                {
                    queries.Add($"{filter.Key} == @{i++}");

                    var propertyDefinition = propertyDefinitions.Where(p => p.Name == filter.Key).FirstOrDefault();

                    //var deserializedReference = ReferenceDeserializer.Get(type.Type, reference, simpleKey);

                    values.Add(filter.Value);
                }

                dbSet = dbSet.Where(string.Join(" OR ", queries), values.ToArray());
            }

            var totalCount = await dbSet.CountAsync().ConfigureAwait(false);

            dbSet = dbSet.Page(page, pageSize);

            var result = new List<ListRow>();

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                var columnValues = new List<string>();

                foreach(var propertyDefinition in selectedPropertyDefinitions)
                {
                    var partialViewName = $"Columns/Text";

                    if (propertyDefinition.Attributes.OfType<SelectAttribute>().Any())
                    {
                        partialViewName = "Columns/Select";
                    }

                    if(type.Type.IsAssignableTo(typeof(INameable)) && propertyDefinition.Name == nameof(INameable.Name))
                    {
                        partialViewName = "Columns/Name";
                    }

                    var viewResult = CompositeViewEngine.FindView(ControllerContext, partialViewName, false);

                    if (!viewResult.Success)
                    {
                        throw new InvalidOperationException($"The partial view '{partialViewName}' was not found. The following locations were searched:{Environment.NewLine}{string.Join(Environment.NewLine, viewResult.SearchedLocations)}");
                    }

                    var view = viewResult.View;

                    using (var writer = new StringWriter())
                    using (view as IDisposable)
                    {
                        var viewData = new ViewDataDictionary(ViewData) { Model = new ListColumnViewModel(type, instance, propertyDefinition, propertyDefinition.Getter(instance)) };

                        var viewContext = new ViewContext(ControllerContext, view, viewData, TempData, writer, new HtmlHelperOptions());
                        await view.RenderAsync(viewContext).ConfigureAwait(false);
                        columnValues.Add(writer.ToString().Trim());
                    }
                }

                result.Add(new ListRow(
                    PrimaryKeyGetter.Get(instance),
                    columnValues
                ));
            }

            return new ListResultResponse(
                result,
                totalCount
            );
        }

        public record ListResultResponse(
            IEnumerable<ListRow> Items,
            int TotalCount
        );

        public record ListRow(
            IEnumerable<object> Keys,
            IEnumerable<string> Values
        );
    }
}
