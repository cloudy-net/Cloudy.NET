using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public record ListColumnCreator(IContentTypeProvider ContentTypeProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter) : IListColumnCreator
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
                        columns.Add(new ListColumnDescriptor(nameof(INameable.Name)));
                    }
                    else
                    {
                        foreach(var primaryKeyProperty in PrimaryKeyPropertyGetter.GetFor(contentType.Type))
                        {
                            columns.Add(new ListColumnDescriptor(primaryKeyProperty.Name));
                        }
                    }
                }
                else
                {
                    foreach (var propertyDefinition in listColumnProperties)
                    {
                        columns.Add(new ListColumnDescriptor(propertyDefinition.Name));
                    }
                }

                result[contentType.Type] = columns;
            }

            return result;
        }
    }
}
