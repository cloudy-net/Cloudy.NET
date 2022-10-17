﻿using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Naming;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

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

                var listColumnProperties = PropertyDefinitionProvider.GetFor(contentType.Name).Where(p => p.Attributes.OfType<ListColumnAttribute>().Any());

                if (!listColumnProperties.Any())
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
                    foreach (var propertyDefinition in listColumnProperties)
                    {
                        var name = propertyDefinition.Name;
                        var humanizedName = Humanizer.Humanize(name);

                        if (propertyDefinition.Attributes.OfType<SelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                        {
                            humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                        }

                        var listColumnAttribute = propertyDefinition.Attributes.OfType<ListColumnAttribute>().First();

                        columns.Add(new ListColumnDescriptor(name, humanizedName, listColumnAttribute.Order == -10000 ? order++ : listColumnAttribute.Order));
                    }
                }

                result[contentType.Type] = columns.OrderBy(c => c.Order);
            }

            return result;
        }
    }
}
