using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
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
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public class ListResultController : Controller
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        ICompositeViewEngine CompositeViewEngine { get; }

        public ListResultController(IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeProvider contentTypeProvider, IContextCreator contextCreator, ICompositeViewEngine compositeViewEngine)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeProvider = contentTypeProvider;
            ContextCreator = contextCreator;
            CompositeViewEngine = compositeViewEngine;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/list/result")]
        public async Task<ListResultResponse> ListResult(string contentType, string columns)
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

            var result = new List<IDictionary<string, string>>();

            var propertyDefinitions = PropertyDefinitionProvider.GetFor(type.Name).Where(p => columnNames.Contains(p.Name));

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                var row = new Dictionary<string, string>();

                foreach(var propertyDefinition in propertyDefinitions)
                {
                    var partialViewName = $"Columns/Text";
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
                        row[propertyDefinition.Name] = writer.ToString().Trim();
                    }
                }

                result.Add(row);
            }

            return new ListResultResponse
            {
                Items = result,
                TotalCount = totalCount,
            };
        }

        public class ListResultPayload
        {
            public List<string> Columns { get; set; }
        }

        public class ListResultResponse
        {
            public IEnumerable<object> Items { get; set; }
            public int TotalCount { get; set; }
        }

        private class MyPage
        {
            public string Name { get; set; }
        }
    }
}
