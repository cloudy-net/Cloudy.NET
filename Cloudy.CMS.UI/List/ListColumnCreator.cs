using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.Naming;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Linq;

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

                var properties = PropertyDefinitionProvider.GetFor(entityType.Name).Where(p => p.Attributes.OfType<ListColumnAttribute>().Any());

                if (!properties.Any())
                {
                    if (entityType.IsNameable)
                    {
                        var name = nameof(INameable.Name);
                        columns.Add(new ListColumnDescriptor(name, Humanizer.Humanize(name), 0, false));
                    }
                    else
                    {
                        var order = 0;
                        foreach(var primaryKeyProperty in PrimaryKeyPropertyGetter.GetFor(entityType.Type))
                        {
                            var name = primaryKeyProperty.Name;
                            columns.Add(new ListColumnDescriptor(name, Humanizer.Humanize(name), order++, false));
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

                        if (propertyDefinition.Attributes.OfType<SelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                        {
                            humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                        }

                        columns.Add(new ListColumnDescriptor(name, humanizedName, attribute.Order == -10000 ? order++ : attribute.Order, attribute.Sortable));
                    }
                }

                result[entityType.Type] = columns.OrderBy(c => c.Order);
            }

            return result;
        }
    }
}
