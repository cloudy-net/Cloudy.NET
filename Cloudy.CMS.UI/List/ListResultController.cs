using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntityTypeSupport;
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
using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntitySupport.Reference;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.UI.FieldSupport.Select;

namespace Cloudy.CMS.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class ListResultController : Controller
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IEntityTypeProvider EntityTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        ICompositeViewEngine CompositeViewEngine { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IReferenceDeserializer ReferenceDeserializer { get; }

        public ListResultController(IPropertyDefinitionProvider propertyDefinitionProvider, IEntityTypeProvider entityTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine, IPrimaryKeyGetter primaryKeyGetter, IReferenceDeserializer referenceDeserializer)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            EntityTypeProvider = entityTypeProvider;
            ContextCreator = contextCreator;
            CompositeViewEngine = compositeViewEngine;
            PrimaryKeyGetter = primaryKeyGetter;
            ReferenceDeserializer = referenceDeserializer;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/list/result")]
        public async Task<ListResultResponse> ListResult(string entityType, string columns, [FromQuery(Name = "filters")]IDictionary<string, string> filters, int page, int pageSize, string search, string orderBy, string orderByDirection)
        {
            var columnNames = columns.Split(",");
            var type = EntityTypeProvider.Get(entityType);
            var propertyDefinitions = PropertyDefinitionProvider.GetFor(type.Name);
            var selectedPropertyDefinitions = columnNames.Select(n => propertyDefinitions.First(p => n == p.Name));
            
            var context = ContextCreator.CreateFor(type.Type);

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

                    var simpleKey = !propertyDefinition.Type.IsAssignableTo(typeof(ITuple));

                    var value = ReferenceDeserializer.Get(type.Type, filter.Value, simpleKey);

                    values.Add(simpleKey ? value.First() : Activator.CreateInstance(propertyDefinition.Type, value));
                }

                dbSet = dbSet.Where(string.Join(" OR ", queries), values.ToArray());
            }

            var totalCount = await dbSet.CountAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(orderBy))
            {
                dbSet = dbSet.OrderBy($"{orderBy} == NULL").ThenBy($"{orderBy} {orderByDirection}");
            }
            
            dbSet = dbSet.Page(page, pageSize);

            var result = new List<ListRow>();

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                var columnValues = new List<string>();

                foreach(var propertyDefinition in selectedPropertyDefinitions)
                {
                    var partialViewName = $"Columns/text";

                    if (propertyDefinition.Attributes.OfType<SelectAttribute>().Any())
                    {
                        partialViewName = "Columns/select";
                    }

                    if (type.Type.IsAssignableTo(typeof(INameable)) && propertyDefinition.Name == nameof(INameable.Name))
                    {
                        partialViewName = "Columns/name";
                    }

                    if (type.Type.IsAssignableTo(typeof(IImageable)) && propertyDefinition.Name == nameof(IImageable.Image))
                    {
                        partialViewName = "Columns/image";
                    }

                    var uiHint = propertyDefinition.Attributes.OfType<ListColumnAttribute>().FirstOrDefault()?.UIHint;

                    if(uiHint != null)
                    {
                        partialViewName = $"Columns/{uiHint}";
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
