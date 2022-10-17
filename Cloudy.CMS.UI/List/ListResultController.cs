using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
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
        public async Task<ListResultResponse> ListResult(string contentType, string columns, int page, int pageSize)
        {
            var columnNames = columns.Split(",");

            var type = ContentTypeProvider.Get(contentType);

            using var context = ContextCreator.CreateFor(type.Type);

            var dbSet = (IQueryable)context.GetDbSet(type.Type).DbSet;

            //if(query != null)
            //{
            //    dbSet = dbSet.Where($"Name.Contains(@0)", query);
            //}

            var totalCount = await dbSet.CountAsync().ConfigureAwait(false);

            dbSet = dbSet.Page(page, pageSize);

            var result = new List<ListRow>();

            var propertyDefinitions = PropertyDefinitionProvider.GetFor(type.Name);

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                var columnValues = new List<string>();

                foreach(var propertyDefinition in columnNames.Select(n => propertyDefinitions.Single(p => n == p.Name)))
                {
                    var partialViewName = $"Columns/Text";

                    if (propertyDefinition.Attributes.OfType<SelectAttribute>().Any())
                    {
                        partialViewName = "Columns/Select";
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
