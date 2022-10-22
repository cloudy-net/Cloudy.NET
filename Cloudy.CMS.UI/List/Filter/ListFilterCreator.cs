using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Naming;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Cloudy.CMS.UI.List.Filter
{
    public record ListFilterCreator(IContentTypeProvider ContentTypeProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IHumanizer Humanizer) : IListFilterCreator
    {
        public IDictionary<Type, IEnumerable<ListFilterDescriptor>> Create()
        {
            var result = new Dictionary<Type, IEnumerable<ListFilterDescriptor>>();

            foreach (var contentType in ContentTypeProvider.GetAll())
            {
                var columns = new List<ListFilterDescriptor>();

                var properties = PropertyDefinitionProvider.GetFor(contentType.Name).Where(p => p.Attributes.OfType<ListFilterAttribute>().Any());

                var order = 10000;
                foreach (var propertyDefinition in properties)
                {
                    var name = propertyDefinition.Name;
                    var humanizedName = Humanizer.Humanize(name);

                    if (propertyDefinition.Attributes.OfType<SelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                    {
                        humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                    }

                    var selectAttribute = propertyDefinition.Attributes.OfType<SelectAttribute>().FirstOrDefault();
                    var select = selectAttribute != null;
                    var selectType = selectAttribute != null ? ContentTypeProvider.Get(selectAttribute.Type)?.Name : null;

                    var attribute = propertyDefinition.Attributes.OfType<ListFilterAttribute>().First();

                    var simpleKey = !propertyDefinition.Type.IsAssignableTo(typeof(ITuple));

                    columns.Add(new ListFilterDescriptor(name, humanizedName, contentType.Name, select, selectType, simpleKey, attribute.Order == -10000 ? order++ : attribute.Order));
                }

                result[contentType.Type] = columns.OrderBy(c => c.Order);
            }

            return result;
        }
    }
}
