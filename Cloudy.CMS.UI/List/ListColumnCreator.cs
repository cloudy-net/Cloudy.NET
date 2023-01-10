using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.Naming;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.UI.List
{
    public record ListColumnCreator(IContentTypeProvider ContentTypeProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter, IHumanizer Humanizer) : IListColumnCreator
    {
        public IDictionary<Type, IEnumerable<ListColumnDescriptor>> Create()
        {
            var result = new Dictionary<Type, IEnumerable<ListColumnDescriptor>>();

            foreach(var contentType in ContentTypeProvider.GetAll())
            {
                var columns = new List<ListColumnDescriptor>();

                var properties = PropertyDefinitionProvider.GetFor(contentType.Name).Where(p => p.Attributes.OfType<ListColumnAttribute>().Any());

                if (!properties.Any())
                {
                    if (contentType.IsNameable)
                    {
                        var name = nameof(INameable.Name);
                        columns.Add(new ListColumnDescriptor(name, Humanizer.Humanize(name), 0));
                    }
                    else
                    {
                        var order = 0;
                        foreach(var primaryKeyProperty in PrimaryKeyPropertyGetter.GetFor(contentType.Type))
                        {
                            var name = primaryKeyProperty.Name;
                            columns.Add(new ListColumnDescriptor(name, Humanizer.Humanize(name), order++));
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

                        columns.Add(new ListColumnDescriptor(name, humanizedName, attribute.Order == -10000 ? order++ : attribute.Order));
                    }
                }

                result[contentType.Type] = columns.OrderBy(c => c.Order);
            }

            return result;
        }
    }
}
