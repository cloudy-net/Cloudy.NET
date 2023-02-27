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
using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS.Naming;

namespace Cloudy.CMS.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class ListResultController : Controller
    {
        INameGetter NameGetter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IEntityTypeProvider EntityTypeProvider { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IReferenceDeserializer ReferenceDeserializer { get; }
        IServiceProvider ServiceProvider { get; }

        public ListResultController(
            IPropertyDefinitionProvider propertyDefinitionProvider,
            IEntityTypeProvider entityTypeProvider,
            IContextCreator contextCreator,
            IPrimaryKeyGetter primaryKeyGetter,
            IReferenceDeserializer referenceDeserializer,
            IServiceProvider serviceProvider,
            INameGetter nameGetter)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            EntityTypeProvider = entityTypeProvider;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
            ReferenceDeserializer = referenceDeserializer;
            ServiceProvider = serviceProvider;
            NameGetter = nameGetter;
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
                var columnInfos = new List<ColumnInfo>();

                foreach(var propertyDefinition in selectedPropertyDefinitions)
                {
                    var partialViewName = $"Columns/text";

                    if (propertyDefinition.Attributes.OfType<ISelectAttribute>().Any())
                    {
                        partialViewName = "Columns/select";
                    }

                    if (propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().Any())
                    {
                        partialViewName = "Columns/customselect";
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
                    
                    columnInfos.Add(
                        new ColumnInfo(
                            await GetFriendlyValue(propertyDefinition, instance),
                            partialViewName,
                            type.IsImageable,
                            type.IsImageable ? ((IImageable)instance).Image : string.Empty
                        )
                    );
                }

                result.Add(new ListRow(
                    PrimaryKeyGetter.Get(instance),
                    columnInfos
                ));
            }

            return new ListResultResponse(
                result,
                totalCount
            );
        }

        private async Task<object> GetFriendlyValue(PropertyDefinitionDescriptor propertyDefinition, object instance)
        {
            var value = propertyDefinition.Getter(instance);
            if (value is null) return null;

            var customSelectAttribute = propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().FirstOrDefault();
            if (customSelectAttribute is not null)
            {
                var factoryType = customSelectAttribute.GetType().GenericTypeArguments.FirstOrDefault();
                var factory = ServiceProvider.GetService(factoryType) as ICustomSelectFactory;
                var items = await factory.GetItems();

                if (propertyDefinition.List)
                {
                    var selectedValues = value as IList<string>;
                    return selectedValues is null ? string.Empty : string.Join(", ", items.Where(i => selectedValues?.Contains(i.Value) ?? false).Select(i => i.Text).Order());
                }
                else
                {
                    return items.FirstOrDefault(x => x.Value == value.ToString())?.Text;
                }
            }

            var selectAttribute = propertyDefinition.Attributes.OfType<ISelectAttribute>().FirstOrDefault();
            if (selectAttribute is not null)
            {
                if (value.Equals(Activator.CreateInstance(value.GetType()))) return null;

                var type = EntityTypeProvider.Get(selectAttribute.Type);
                var context = ContextCreator.CreateFor(type.Type);
                var relatedInstance = await context.Context.FindAsync(type.Type, value).ConfigureAwait(false);
                var name = NameGetter.GetName(relatedInstance);
                return new { name = name ?? "Does not exist", image = (relatedInstance as IImageable)?.Image };
            }

            return value;
        }

        public record ListResultResponse(
            IEnumerable<ListRow> Items,
            int TotalCount
        );

        public record ListRow(
            IEnumerable<object> Keys,
            IEnumerable<ColumnInfo> Values
        );

        public record ColumnInfo(
            object Value,
            string Partial,
            bool IsImageable,
            string Image
        );
    }
}
