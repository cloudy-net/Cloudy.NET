using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntitySupport;
using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.EntitySupport.Reference;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.PropertyDefinitionSupport;
using Cloudy.NET.UI.FieldSupport.CustomSelect;
using Cloudy.NET.UI.FieldSupport.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class ListResultController : Controller
    {
        IColumnValueProvider ColumnValueProvider { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IEntityTypeProvider EntityTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IReferenceDeserializer ReferenceDeserializer { get; }

        public ListResultController(
            IPropertyDefinitionProvider propertyDefinitionProvider,
            IEntityTypeProvider entityTypeProvider,
            IContextCreator contextCreator,
            IPrimaryKeyGetter primaryKeyGetter,
            IReferenceDeserializer referenceDeserializer,
            IColumnValueProvider columnValueProvider)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            EntityTypeProvider = entityTypeProvider;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
            ReferenceDeserializer = referenceDeserializer;
            ColumnValueProvider = columnValueProvider;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/list/result")]
        public async Task<ListResultResponse> ListResult(string entityType, string columns, [FromQuery(Name = "filters")] IDictionary<string, string> filters, int page, int pageSize, string search, string orderBy, string orderByDirection)
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

                foreach (var filter in filters)
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

            var result = new List<ListItem>();

            await foreach (var instance in (IAsyncEnumerable<object>)dbSet)
            {
                var value = new Dictionary<string, object>();

                foreach (var propertyDefinition in selectedPropertyDefinitions)
                {
                    value[propertyDefinition.Name] = await ColumnValueProvider.Get(propertyDefinition, instance);
                }

                result.Add(new ListItem(PrimaryKeyGetter.Get(instance), value));
            }

            return new ListResultResponse(
                result,
                totalCount
            );
        }

        public record ListResultResponse(
            IEnumerable<ListItem> Items,
            int TotalCount
        );

        public record ListItem(
            IEnumerable<object> Keys,
            IDictionary<string, object> Value
        );
    }
}
