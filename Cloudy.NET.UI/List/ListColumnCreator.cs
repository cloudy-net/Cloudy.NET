using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.Naming;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using Cloudy.CMS.UI.FieldSupport.CustomSelect;

namespace Cloudy.CMS.UI.List
{
    public record ListColumnCreator(IEntityTypeProvider EntityTypeProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter, IHumanizer Humanizer) : IListColumnCreator
    {
        public IDictionary<Type, IEnumerable<ListColumnDescriptor>> Create()
        {
            var result = new Dictionary<Type, IEnumerable<ListColumnDescriptor>>();

            foreach(var entityType in EntityTypeProvider.GetAll().Where(t => t.IsIndependent))
            {
                var columns = new List<ListColumnDescriptor>();

                var allProperties = PropertyDefinitionProvider.GetFor(entityType.Name);
                var properties = allProperties.Where(p => p.Attributes.OfType<ListColumnAttribute>().Any());

                if (!properties.Any())
                {
                    if (entityType.IsNameable)
                    {
                        var name = nameof(INameable.Name);
                        columns.Add(new ListColumnDescriptor(name, Humanizer.Humanize(name), GetPartial(entityType.Type, allProperties.First(p => p.Name == name)), 0, false, ListingColumnWidth.Default, true));
                    }
                    else
                    {
                        var order = 0;
                        foreach(var primaryKeyProperty in PrimaryKeyPropertyGetter.GetFor(entityType.Type))
                        {
                            var name = primaryKeyProperty.Name;
                            columns.Add(new ListColumnDescriptor(name, Humanizer.Humanize(name), GetPartial(entityType.Type, allProperties.First(p => p.Name == name)), order++, false, ListingColumnWidth.Default, true));
                        }
                    }
                }
                else
                {
                    var order = 10000;
                    foreach (var propertyDefinition in properties)
                    {
                        var attribute = propertyDefinition.Attributes.OfType<ListColumnAttribute>().First();

                        var name = propertyDefinition.Name;
                        var humanizedName = attribute.Name ?? Humanizer.Humanize(name);

                        if (propertyDefinition.Attributes.OfType<ISelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                        {
                            humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                        }

                        columns.Add(new ListColumnDescriptor(name, humanizedName, GetPartial(entityType.Type, propertyDefinition), attribute.Order == -10000 ? order++ : attribute.Order, attribute.Sortable, attribute.Width, attribute.ShowInCompactView ?? entityType.IsNameable && propertyDefinition.Name == nameof(INameable.Name) || false));
                    }
                }

                result[entityType.Type] = columns.OrderBy(c => c.Order);
            }

            return result;
        }

        string GetPartial(Type type, PropertyDefinitionDescriptor propertyDefinition)
        {
            var partialViewName = $"columns/text";

            if (propertyDefinition.Attributes.OfType<ISelectAttribute>().Any())
            {
                partialViewName = "columns/select";
            }

            if (propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().Any())
            {
                partialViewName = "columns/customselect";
            }

            if (type.IsAssignableTo(typeof(INameable)) && propertyDefinition.Name == nameof(INameable.Name))
            {
                partialViewName = "columns/name";
            }

            if (type.IsAssignableTo(typeof(IImageable)) && propertyDefinition.Name == nameof(IImageable.Image))
            {
                partialViewName = "columns/image";
            }

            var uiHint = propertyDefinition.Attributes.OfType<ListColumnAttribute>().FirstOrDefault()?.UIHint;

            if (uiHint != null)
            {
                partialViewName = uiHint;
            }

            return $"{partialViewName}.js";
        }
    }
}
